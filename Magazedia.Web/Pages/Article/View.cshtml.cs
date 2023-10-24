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
	public string? ArticleHtmlTitle { get; set; }
	public bool ArticleFound { get; set; }

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

		string ArticleRevisionDate = "";

		// This page can be accessed by UrlSlug or by ID of Article
		if (Id is not null)
		{
			// TODO: Check ID actually exists and error if not

			// Article revision look-up by ID
			SqlQuery = @"
						SELECT		ar.Id, ar.ArticleId, a.Title, a.UrlSlug, ar.[Text], ar.RevisionReason, u.Id as CreatedByAspNetUserId, u.UserName as CreatedByAspNetUsername, ar.DateCreated, ar.DateDeleted
						FROM		Articles a
						INNER JOIN	ArticleRevisions ar ON a.Id = ar.ArticleId
						INNER JOIN	AspNetUsers u ON ar.CreatedByAspNetUserId = u.Id
						WHERE		ar.Id = @Id AND
									a.DateDeleted IS NULL AND
									ar.DateDeleted IS NULL;
						";
			ArticleRevision = Connection.QuerySingleOrDefault<ArticleRevision>(SqlQuery, new { Id });

			ArticleRevisionDate = " (Prior revision dated " + ArticleRevision.DateCreated.ToString("dddd dd MMMM yyyy HH:mm:ss") + " -- @" + Helpers.ConvertDateTimeToBeatsInternetTime(ArticleRevision.DateCreated) + ")";

			ArticleFound = true;
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

			ArticleFound = true;
		}

		// Check if Article exists
		if (ArticleRevision is null)
		{
			ArticleFound = false;
			// Article does not exist in database

			// As the Article doesn't exist we don't want it to show up in SERPs
			this.AllowSearchEngineIndexing = false;

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
				ArticleText = $"<p>User profiles not currently implemented.</p>";
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
			ArticleTitle = ArticleRevision.Title + ArticleRevisionDate;

			ArticleHtmlTitle = ArticleRevision.Title + ArticleRevisionDate;

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

			ImageExtension ImageExtension = new(SiteId, Connection, this);
			ShortDescriptionExtension ShortDescriptionExtension = new(this);

			// There are some types of tags which do not render at the point they are inserted but
			// are collected for rendering in blocks, such as Alerts, Citations, Footnotes and Categories

			// Empty lists for collecting the non-rendering tags' data
			List<WikiWikiWorld.Models.Alert> Alerts = new();
			List<WikiWikiWorld.Models.Citation> Citations = new();
			List<WikiWikiWorld.Models.Footnote> Footnotes = new();
			List<WikiWikiWorld.Models.Category> Categories = new();

			DownloadsBoxExtension DownloadBoxExtension = new(SiteId, Connection);

			// Send the empty list into the CitationExtension - if the parser finds any citations in the Article
			// it will not render them, but will simply add them to the Citations List
			CitationExtension CitationExtension = new(Citations);

			// Send the Citations List into the CitationsExtension - if the parser finds a {{Citations}} in the Article
			// it will render out a list of all the citations
			CitationsExtension CitationsExtension = new(Citations);

			FootnoteExtension FootnoteExtension = new(Footnotes);
			FootnotesExtension FootnotesExtension = new(Footnotes);

			CategoryExtension CategoryExtension = new(Categories);
			CategoriesExtension CategoriesExtension = new(Categories);

			// Stubs can add an Alert at the top of the page and a Category at the foot of the page
			StubExtension StubExtension = new(Alerts, Categories);

			AlertsExtension AlertsExtension = new(Alerts);

			MagazineInfoboxExtension MagazineInfoboxExtension = new(SiteId, Connection);

			var builder = new MarkdownPipelineBuilder().UseAdvancedExtensions();

			// Don't sap link juice or follow the links from old revisions
			// This makes all the links in the article rel="nofollow"
			if (Id is not null)
			{
				builder = builder.UseReferralLinks(new[] { "nofollow" });
			}

			var pipeline = builder
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
				.Use(AlertsExtension)
				.Use(MagazineInfoboxExtension)
				.Use(DownloadBoxExtension)
				.Build();

			if (ArticleRevision.UrlSlug!.StartsWith("image:"))
			{
				SqlQuery = @"	
						SELECT 		*
						FROM		FileRevisions
						WHERE		ArticleId = @ArticleId AND
									DateDeleted IS NULL
						ORDER BY	DateCreated DESC
						";
				List<FileRevision> FileRevisions = Connection.Query<FileRevision>(SqlQuery, new { ArticleRevision.ArticleId }).ToList();

				ArticleText = Markdown.ToHtml(ArticleRevision.Text, pipeline) + "<br /><img src='/sitefiles/" + SiteId + "/images/" + FileRevisions[0].FileName + "' />";
				if (MetaDescription != null)
				{
					MetaDescription += ArticleRevisionDate;
				}
			}
			else
			{
				ArticleText = Markdown.ToHtml(ArticleRevision.Text, pipeline);
				if (MetaDescription != null)
				{
					MetaDescription += ArticleRevisionDate;
				}
			}
		}



		return Page();
	}
}

