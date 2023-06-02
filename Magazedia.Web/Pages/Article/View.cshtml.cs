using Markdig;
using MarkdigMantisLink;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;
using Dapper;
using WikiWikiWorld.Models;
using System.Globalization;

namespace Magazedia.Web.Pages;
public class ArticleViewModel : PageModel
{
	public string? ArticleTitle { get; set; }
	public string? ArticleText { get; set; }

	[BindProperty(SupportsGet = true)]
	public string? UrlSlug { get; set; }

	[BindProperty(SupportsGet = true)]
	public string? Id { get; set; }

	public IList<ArticleCultureLink>? ArticleCultureLinks { get; set; }

	private readonly IConfiguration Config;
	private readonly IHttpContextAccessor HttpContextAccessor;
	public readonly string Culture;

	public ArticleViewModel(IConfiguration Config, IHttpContextAccessor HttpContextAccessor)
	{
		this.Config = Config;
		this.HttpContextAccessor = HttpContextAccessor;
		Culture = Magazedia.Helpers.GetCultureFromHostname(HttpContextAccessor.HttpContext!.Request.Host.Host, "en");
	}

	public IActionResult OnGet()
	{
		int SiteId = 1;

		using var Connection = new SqlConnection(Config.GetConnectionString("DefaultConnection"));

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
						SELECT		ar.Id, a.Title, a.UrlSlug, ar.[Text], ar.RevisionReason, ar.DateCreated, u.UserName as CreatorUsername
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


		if (ArticleRevision is null)
		{
			CultureInfo CultureInfo = new CultureInfo(Culture.Equals("xx-test") ? "zh" : Culture);
			string TitleHint = CultureInfo.TextInfo.ToTitleCase(UrlSlug!.Replace('-', ' '));

			if (UrlSlug!.StartsWith("category:"))
			{
				TitleHint = CultureInfo.TextInfo.ToTitleCase((UrlSlug!.Split(":")[1]).Replace('-', ' '));
				ArticleText = $"<p>Article not found. <a href=\"create:{UrlSlug!}?titlehint={TitleHint}\">Create new category article about &ldquo;{TitleHint}&rdquo;</a>.</p>";
			}
			else
			{
				TitleHint = CultureInfo.TextInfo.ToTitleCase(UrlSlug!.Replace('-', ' '));
				ArticleText = $"<p>Article not found. <a href=\"create:{UrlSlug!}?titlehint={TitleHint}\">Create new article about &ldquo;{TitleHint}&rdquo;</a>.</p>";
			}

			ArticleTitle = "Article not found";
		}
		else
		{

			ArticleTitle = ArticleRevision.Title;

			// Find and display any alternate versions of this article in other languages
			SqlQuery = @"	SELECT		A.Title, A.UrlSlug, A.Culture
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


			var pipeline = new MarkdownPipelineBuilder().UseAdvancedExtensions()
				.UseMantisLinks(new MantisLinkOptions("https://issues.company.net/"))
				.Build();

			if (ArticleRevision.UrlSlug.StartsWith("file:"))
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

