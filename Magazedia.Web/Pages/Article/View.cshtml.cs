using Markdig;
using MarkdigMantisLink;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;
using Dapper;
using WikiWikiWorld.Models;
using System.Globalization;
using WikiWikiWorld.MarkdigExtensions;

namespace Magazedia.Web.Pages;
public class ArticleViewModel : BasePageModel
{
	public string? ArticleTitle { get; set; }
	public string? ArticleText { get; set; }

	[BindProperty(SupportsGet = true)]
	public string? UrlSlug { get; set; }

	[BindProperty(SupportsGet = true)]
	public string? Id { get; set; }

	public IList<ArticleCultureLink>? ArticleCultureLinks { get; set; }

	public IList<ArticleTalkSubject>? ArticleTalkSubjects { get; set; }

	public ArticleViewModel(IConfiguration Configuration, IHttpContextAccessor HttpContextAccessor) : base(Configuration, HttpContextAccessor) { }


	public IActionResult OnGet()
	{
		using var Connection = new SqlConnection(Configuration.GetConnectionString("DefaultConnection"));

		string SqlQuery = "";
		ArticleRevision? ArticleRevision = null;

		// This page can be accessed by UrlSlug or by ID of Article
		if (Id is not null)
		{
			// Article revision look-up by ID
			SqlQuery = @"
						SELECT		ar.Id, a.Title, a.UrlSlug, ar.[Text], ar.RevisionReason, ar.DateCreated, u.UserName as CreatorUsername
						FROM		Articles a
						INNER JOIN	ArticleRevisions ar ON a.Id = ar.ArticleId
						INNER JOIN	AspNetUsers u ON ar.CreatedByAspNetUserId = u.Id
						WHERE		ar.Id = @Id AND
									a.DateDeleted IS NULL AND
									ar.DateDeleted IS NULL;
						";
			ArticleRevision = Connection.QuerySingleOrDefault<ArticleRevision>(SqlQuery, new { Id });
		}
		else
		{
			// Article (most recent revision) look-up by UrlSlug
			SqlQuery = @"
						SELECT		ar.Id, ar.ArticleId, a.Title, a.UrlSlug, ar.[Text], ar.RevisionReason, ar.CreatedByAspNetUserId, u.UserName as CreatedByAspNetUsername, ar.DateCreated, ar.DateDeleted
						FROM		Articles a
						INNER JOIN	ArticleRevisions ar ON a.Id = ar.ArticleId
						INNER JOIN	AspNetUsers u ON ar.CreatedByAspNetUserId = u.Id
						WHERE		a.UrlSlug = @UrlSlug AND
									a.SiteId = @SiteId AND
									a.Culture = @Culture AND
									a.DateDeleted IS NULL AND
									ar.DateDeleted IS NULL AND
									ar.DateCreated =
									(
										SELECT	MAX(DateCreated)
										FROM	ArticleRevisions
										WHERE	ArticleId = a.Id
									);
						";

			ArticleRevision = Connection.QuerySingleOrDefault<ArticleRevision>(SqlQuery, new { UrlSlug, SiteId, Culture });
		}

		// Check if Article exists
		if (ArticleRevision is null)
		{
			// Article does not exist in database

			// Convert the current site Culture into a CultureInfo object; if we are on the test Culture then use zh (Chinese)
			CultureInfo CultureInfo = new CultureInfo(Culture.Equals("xx-test") ? "zh" : Culture);
			string TitleHint = CultureInfo.TextInfo.ToTitleCase(UrlSlug!.Replace('-', ' '));

			if (UrlSlug!.StartsWith("category:"))
			{
				TitleHint = CultureInfo.TextInfo.ToTitleCase((UrlSlug!.Split(":")[1]).Replace('-', ' '));
				ArticleText = $"<p>Article not found. <a href=\"/create:{UrlSlug!}?titlehint={TitleHint}\">Create new category article about &ldquo;{TitleHint}&rdquo;</a>.</p>";
			}
			else if (UrlSlug!.StartsWith("@"))
			{
				ArticleText = $"<p>User {UrlSlug!} not found.</p>";
			}
			else
			{
				TitleHint = CultureInfo.TextInfo.ToTitleCase(UrlSlug!.Replace('-', ' '));
				ArticleText = $"<p>Article not found. <a href=\"/create:{UrlSlug!}?titlehint={TitleHint}\">Create new article about &ldquo;{TitleHint}&rdquo;</a>.</p>";
			}

			ArticleTitle = "Article not found";
		}
		else
		{
			// Article exists in database
			ArticleTitle = ArticleRevision.Title;

			// Find and display any alternate versions of this Article in other cultures
			SqlQuery = @"	
						SELECT		A.Title, A.UrlSlug, A.Culture
						FROM		Articles A
						JOIN		ArticleCultureLinks ACL ON A.Id = ACL.ArticleId
						WHERE		ACL.ArticleCultureLinkGroupId IN
									(
										SELECT	ACLG.ArticleCultureLinkGroupId
										FROM	Articles A
										JOIN	ArticleCultureLinks ACLG ON A.Id = ACLG.ArticleId
										WHERE	A.SiteId = @SiteId AND
												A.Culture = @Culture AND
												A.UrlSlug = @UrlSlug
									) AND
									A.DateDeleted IS NULL AND
									ACL.DateDeleted IS NULL;
						";
			ArticleCultureLinks = Connection.Query<WikiWikiWorld.Models.ArticleCultureLink>(SqlQuery, new { SiteId, Culture, UrlSlug }).ToList();

			// Find and display any Talk subjects for this Article
			SqlQuery = @"	
						SELECT		*
						FROM		ArticleTalkSubjects
						WHERE		ArticleId = @ArticleId AND
									SiteId = @SiteId;
						";
			ArticleTalkSubjects = Connection.Query<WikiWikiWorld.Models.ArticleTalkSubject>(SqlQuery, new { SiteId, ArticleRevision.ArticleId }).ToList();

			ImageExtension ImageExtension = new(SiteId, Connection);
			ShortDescriptionExtension ShortDescriptionExtension = new(this);

			// Create an empty list of Citations
			List<WikiWikiWorld.Models.Citation> CitationList = new();

			// Send the empty list into the CitationExtension - if the parser finds any citations in the Article
			// it will not render them, but will simply add them to the CitationList
			CitationExtension CitationExtension = new(CitationList);

			// Send the CitationList into the CitationsExtension - if the parser finds a citations block in the Article
			// it will render out a list of all the citations
			CitationsExtension CitationsExtension = new(CitationList);

			List<WikiWikiWorld.Models.Footnote> FootnoteList = new();
			FootnoteExtension FootnoteExtension = new(FootnoteList);
			FootnotesExtension FootnotesExtension = new(FootnoteList);

			List<WikiWikiWorld.Models.Category> CategoryList = new();
			CategoryExtension CategoryExtension = new(CategoryList);
			CategoriesExtension CategoriesExtension = new(CategoryList);
			StubExtension StubExtension = new(CategoryList);

			var pipeline = new MarkdownPipelineBuilder().UseAdvancedExtensions()
				.UseMantisLinks(new MantisLinkOptions("https://issues.company.net/"))
				.Use(ImageExtension)
				.Use(ShortDescriptionExtension)
				.Use(CategoryExtension)
				.Use(CategoriesExtension)
				.Use(StubExtension)
				.Use(CitationExtension)
				.Use(CitationsExtension)
				.Use(FootnoteExtension)
				.Use(FootnotesExtension)
				.Build();

			if (ArticleRevision.UrlSlug!.StartsWith("file:"))
			{
				ArticleText = Markdown.ToHtml(ArticleRevision.Text, pipeline) + "<br /><img src='/sitefiles/" + SiteId + "/" + ArticleRevision.UrlSlug.Substring(5) + "' />";
			}
			else
			{
				ArticleText = Markdown.ToHtml(ArticleRevision.Text, pipeline);
			}
		}



		return Page();
	}
}

