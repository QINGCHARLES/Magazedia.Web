using Markdig;
using MarkdigMantisLink;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;
using Dapper;
using WikiWikiWorld.Models;

namespace Magazedia.Web.Pages;
public class ArticleModel : PageModel
{
	public string? ArticleTitle { get; set; }
	public string? ArticleText { get; set; }

	[BindProperty(SupportsGet = true)]
	public string? UrlSlug { get; set; }

	[BindProperty(SupportsGet = true)]
	public string? Id { get; set; }

	public IList<ArticleLanguageLink>? ArticleLanguageLinks { get; set; }

	private readonly IConfiguration Config;
	private readonly IHttpContextAccessor HttpContextAccessor;
	public readonly string Language;

	public ArticleModel(IConfiguration Config, IHttpContextAccessor HttpContextAccessor)
	{
		this.Config = Config;
		this.HttpContextAccessor = HttpContextAccessor;
		Language = "en";// Magazedia.Helpers.GetLanguage(HttpContext.Request.Host.Host);
		Language = Magazedia.Helpers.GetLanguage(HttpContextAccessor.HttpContext!.Request.Host.Host);
	}

	public IActionResult OnGet()
	{
		using var Connection = new SqlConnection(Config.GetConnectionString("DefaultConnection"));

		string SqlQuery = "";
		Article? Article = null;

		// This page can be accessed by UrlSlug or by Id of Article
		if (Id is not null)
		{
			SqlQuery = "SELECT * FROM Articles WHERE Id = @Id AND Language = @Language AND DateDeleted IS NULL ORDER BY DateCreated DESC";
			Article = Connection.QuerySingleOrDefault<Article>(SqlQuery, new { Id = Id, Language = Language });
		}
		else
		{
			SqlQuery = "SELECT TOP(1) * FROM Articles WHERE UrlSlug = @UrlSlug AND Language = @Language AND DateDeleted IS NULL ORDER BY DateCreated DESC";
			Article = Connection.QuerySingleOrDefault<Article>(SqlQuery, new { UrlSlug = UrlSlug, Language = Language });
		}


		if (Article is null)
		{
			return NotFound();
		}

		ArticleTitle = Article.Title;

		SqlQuery = @"	SELECT	Link.*, Articles.UrlSlug AS ArticleUrlSlug
						FROM	ArticleLanguageLinks AS Article
						JOIN	ArticleLanguageLinks AS Link ON	Article.ArticleLanguageGroupId = Link.ArticleLanguageGroupId
						JOIN	Articles ON Link.ArticleTitle = Articles.Title
						WHERE	Article.ArticleTitle = @ArticleTitle AND
								Article.SiteId = @SiteId AND
								Article.Language = @Language AND
								Article.DateDeleted IS NULL";
		ArticleLanguageLinks = Connection.Query<WikiWikiWorld.Models.ArticleLanguageLink>(SqlQuery, new { ArticleTitle, SiteId = 1, Language }).ToList();


		var pipeline = new MarkdownPipelineBuilder().UseAdvancedExtensions()
			.UseMantisLinks(new MantisLinkOptions("https://issues.company.net/"))
			.Build();

		
		ArticleText = Markdown.ToHtml(Article.Text, pipeline);

		return Page();
	}
}

