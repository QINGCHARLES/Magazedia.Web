using Markdig;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;
using Dapper;
using WikiWikiWorld.Models;

namespace Magazedia.Web.Pages
{
	public class HistoryModel : PageModel
	{
		public string? ArticleTitle { get; set; }

		[BindProperty(SupportsGet = true)]
		public string? UrlSlug { get; set; }

		public IList<Article>? Articles { get; set; }

		private readonly IConfiguration Config;
		private readonly string Language;
		public HistoryModel(IConfiguration Config)
		{
			this.Config = Config;
			Language = "en";// Magazedia.Helpers.GetLanguage(HttpContext.Request.Host.Host);
		}

		public IActionResult OnGet()
		{
			using var Connection = new SqlConnection(Config.GetConnectionString("DefaultConnection"));

			string SqlQuery = "SELECT * FROM Article WHERE UrlSlug = @UrlSlug AND Language = @Language ORDER BY DateCreated DESC";
			Articles = Connection.Query<Article>(SqlQuery, new { UrlSlug = UrlSlug, Language = Language }).ToList();
			//Articles = ArticlesList;
			ArticleTitle = Articles[0].Title;

			return Page();
		}
	}
}

