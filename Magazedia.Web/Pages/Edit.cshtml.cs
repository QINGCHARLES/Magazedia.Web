using Markdig;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;
using Dapper;
using WikiWikiWorld.Models;
using System.Security.Claims;

namespace Magazedia.Web.Pages;
public class EditModel : PageModel
{
	public string? ArticleTitle { get; set; }
	public string? ArticleUrlSlug { get; set; }

	public string? ArticleHtml { get; set; }
	public string? ArticleRevisionReason { get; set; }
	public string? ArticleText { get; set; }

	[BindProperty(SupportsGet = true)]
	public string? UrlSlug { get; set; }

	private readonly IConfiguration Config;
	private readonly IHttpContextAccessor HttpContextAccessor;
	private readonly string Language;
	public EditModel(IConfiguration Config, IHttpContextAccessor HttpContextAccessor)
	{
		this.Config = Config;
		this.HttpContextAccessor = HttpContextAccessor;
		Language = "en";// Magazedia.Helpers.GetLanguage(HttpContext.Request.Host.Host);
	}
	public IActionResult OnPost()
	{
		ArticleText = Request.Form[nameof(ArticleText)];
		ArticleRevisionReason = Request.Form[nameof(ArticleRevisionReason)];

		ClaimsPrincipal? user = HttpContextAccessor.HttpContext?.User;
		string Username = user?.Identity?.Name ?? "Anonymous";

		using var Connection = new SqlConnection(Config.GetConnectionString("DefaultConnection"));

		var SqlQuery = "SELECT TOP(1) * FROM Article WHERE UrlSlug = @UrlSlug AND Language = @Language AND DateDeleted IS NULL ORDER BY DateCreated DESC";
		var Article = Connection.QuerySingleOrDefault(SqlQuery, new { UrlSlug = UrlSlug, Language = Language });

		SqlQuery = "INSERT Article (Title, UrlSlug, [Text], RevisionReason, CreatedByAspNetUserId, SiteId, Language) VALUES (@Title, @UrlSlug, @Text, @RevisionReason, @CreatedByAspNetUserId, @SiteId, @Language);";
		var res = Connection.Execute( SqlQuery, new { Title = Article.Title, UrlSlug = Article.UrlSlug, Text = ArticleText, RevisionReason = ArticleRevisionReason, CreatedByAspNetUserId = Username, SiteId = 1, Language = Language });

		return Content("output:" + res.ToString() + UrlSlug + ArticleText);
	}
	public IActionResult OnGet()
	{
		using var Connection = new SqlConnection(Config.GetConnectionString("DefaultConnection"));
		string SqlQuery = "SELECT TOP(1) * FROM Article WHERE UrlSlug = @UrlSlug AND Language = @Language AND DateDeleted IS NULL ORDER BY DateCreated DESC";
		var Article = Connection.QuerySingleOrDefault(SqlQuery, new { UrlSlug = UrlSlug, Language = Language });

		if (Article is null)
		{
			return NotFound();
		}

		var pipeline = new MarkdownPipelineBuilder().UseAdvancedExtensions().Build();

		ArticleTitle = Article.Title;
		ArticleUrlSlug = Article.UrlSlug;
		ArticleText = Article.Text;
		ArticleHtml = Markdown.ToHtml(Article.Text, pipeline);

		return Page();
	}
}

