using Markdig;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;
using Dapper;
using WikiWikiWorld.Models;
using System.Security.Claims;
using System.Text.RegularExpressions;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Security.Policy;

namespace Magazedia.Web.Pages;
public class EditModel : PageModel
{
	public string? ArticleTitle { get; set; }
	public string? ArticleUrlSlug { get; set; }
	public string? ArticleHtml { get; set; }
	public string? ArticleRevisionReason { get; set; }
	public string? ArticleText { get; set; }

	public WikiWikiWorld.Models.ArticleRevision? ArticleRevision { get; set; }

	[BindProperty(SupportsGet = true)]
	public string? UrlSlug { get; set; }

	private readonly IConfiguration Config;
	private readonly IHttpContextAccessor HttpContextAccessor;
	private readonly string Culture;

	public EditModel(IConfiguration Config, IHttpContextAccessor HttpContextAccessor)
	{
		this.Config = Config;
		this.HttpContextAccessor = HttpContextAccessor;
		Culture = Magazedia.Helpers.GetCultureFromHostname(HttpContextAccessor.HttpContext!.Request.Host.Host, "en");
	}

	public IActionResult OnPost()
	{
		ArticleTitle = Request.Form[nameof(ArticleTitle)];
		ArticleText = Request.Form[nameof(ArticleText)];
		ArticleUrlSlug = Request.Form[nameof(ArticleUrlSlug)];
		ArticleRevisionReason = Request.Form[nameof(ArticleRevisionReason)];

		int SiteId = 1;

		ClaimsPrincipal? User = HttpContextAccessor.HttpContext?.User;
		string Username = User?.FindFirstValue(ClaimTypes.NameIdentifier) ?? "7240be61-df81-46f9-8152-6a48b96abc40";// "Anonymous";

		using var Connection = new SqlConnection(Config.GetConnectionString("DefaultConnection"));

		string SqlQuery = "SELECT * FROM Articles WHERE UrlSlug = @UrlSlug AND Culture = @Culture AND SiteId = @SiteId AND DateDeleted IS NULL";
		var Article = Connection.QuerySingleOrDefault(SqlQuery, new { UrlSlug = ArticleUrlSlug, SiteId, Culture });


			SqlQuery = @"	INSERT ArticleRevisions (ArticleId, [Text], RevisionReason, CreatedByAspNetUserId)
							VALUES (@ArticleId, @Text, @RevisionReason, @CreatedByAspNetUserId);
						";
			var res = Connection.Execute(SqlQuery, new { @ArticleId = Article.Id, Text = ArticleText, RevisionReason = ArticleRevisionReason, CreatedByAspNetUserId = Username });



		return Content("output:" + res.ToString() + UrlSlug + ArticleText);
	}
	public IActionResult OnGet()
	{
		using var Connection = new SqlConnection(Config.GetConnectionString("DefaultConnection"));
		int SiteId = 1;
		string SqlQuery = @"
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

		//Article = Connection.QuerySingleOrDefault<Article>(SqlQuery, new { UrlSlug = UrlSlug, Language = Language });

		if (ArticleRevision is null)
		{
			return NotFound();
		}

		var Pipeline = new MarkdownPipelineBuilder().UseAdvancedExtensions().Build();

		ArticleTitle = ArticleRevision.Title;
		ArticleUrlSlug = ArticleRevision.UrlSlug;
		ArticleText = ArticleRevision.Text;
		ArticleHtml = Markdown.ToHtml(ArticleRevision.Text, Pipeline);

		return Page();
	}
}

