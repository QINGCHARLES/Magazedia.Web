using Markdig;
using MarkdigMantisLink;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;
using Dapper;
using WikiWikiWorld.Models;

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
		//Culture = "en";// Magazedia.Helpers.GetLanguage(HttpContext.Request.Host.Host);
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
			// Article lookup by ID
			SqlQuery = @"
						SELECT		ar.Id, a.Title, a.UrlSlug, ar.[Text], ar.RevisionReason, ar.DateCreated, u.UserName as CreatorUsername
						FROM		Articles a
						INNER JOIN	ArticleRevisions ar ON a.Id = ar.ArticleId
						INNER JOIN	AspNetUsers u ON ar.CreatedByAspNetUserId = u.Id
						WHERE		ar.Id = @Id AND
									a.DateDeleted IS NULL AND
									ar.DateDeleted IS NULL;
						";
			ArticleRevision = Connection.QuerySingleOrDefault<ArticleRevision>(SqlQuery, new { Id = Id, Language = Culture });
		}
		else
		{
			// Article lookup by UrlSlug
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

			//SELECT TOP(1) * FROM Articles WHERE UrlSlug = @UrlSlug AND Language = @Language AND DateDeleted IS NULL ORDER BY DateCreated DESC";
			ArticleRevision = Connection.QuerySingleOrDefault<ArticleRevision>(SqlQuery, new { UrlSlug, SiteId, Culture });
		}


		if (ArticleRevision is null)
		{
			return NotFound();
		}

		ArticleTitle = ArticleRevision.Title;

		SqlQuery = @"	SELECT		A.Title, A.UrlSlug, A.Culture
						FROM		Articles A
						JOIN		ArticleCultureLinks ACL ON A.Id = ACL.ArticleId
						WHERE		ACL.ArticleCultureLinkGroupId
									IN (
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

		
		ArticleText = Markdown.ToHtml(ArticleRevision.Text, pipeline);

		return Page();
	}
}

