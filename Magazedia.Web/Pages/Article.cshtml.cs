using Markdig;
using MarkdigMantisLink;
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

	[BindProperty(SupportsGet = true)]
	public string? Id { get; set; }

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

		string SqlQuery = "";
WikiWikiWorld.Models.Article? Article = null;
		
if( Id is not null )
{
	SqlQuery = "SELECT * FROM Articles WHERE Id = @Id AND Language = @Language AND DateDeleted IS NULL ORDER BY DateCreated DESC";
	Article = Connection.QuerySingleOrDefault<WikiWikiWorld.Models.Article>(SqlQuery, new { Id = Id, Language = Language });
}
else
{
	SqlQuery = "SELECT TOP(1) * FROM Articles WHERE UrlSlug = @UrlSlug AND Language = @Language AND DateDeleted IS NULL ORDER BY DateCreated DESC";
	Article = Connection.QuerySingleOrDefault<WikiWikiWorld.Models.Article>(SqlQuery, new { UrlSlug = UrlSlug, Language = Language });
}


		if (Article is null)
		{
			return NotFound();
		}

		var pipeline = new MarkdownPipelineBuilder().UseAdvancedExtensions()
			.UseMantisLinks(new MantisLinkOptions("https://issues.company.net/"))
			.Build();

		ArticleTitle = Article.Title;
		ArticleText = Markdown.ToHtml(Article.Text, pipeline);

		return Page();
	}
}

