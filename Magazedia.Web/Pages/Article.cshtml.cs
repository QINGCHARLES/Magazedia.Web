using Markdig;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;
using Dapper;

namespace Magazedia.Web.Pages;
public class ArticleModel : PageModel
{
	public string? ArticleTitle { get; set; }
	public string? ArticleText { get; set; }

	[BindProperty(SupportsGet = true)]
	public string? UrlSlug { get; set; }

	private readonly IConfiguration Config;
	private readonly string Language;

	public ArticleModel(IConfiguration Config)
	{
		this.Config = Config;
		Language = "en";// Magazedia.Helpers.GetLanguage(HttpContext.Request.Host.Host);
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
		ArticleText = Markdown.ToHtml(Article.Text, pipeline);

		return Page();
	}
}

