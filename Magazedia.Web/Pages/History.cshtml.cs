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

			// Get a list of all the revisions of this Article and convert the UserId in the Article table to a Username for display
			string SqlQuery = @"SELECT		Articles.*, AspNetUsers.UserName AS CreatedByAspNetUsername
								FROM		Articles
								INNER JOIN	AspNetUsers ON Articles.CreatedByAspNetUserId = AspNetUsers.Id
								WHERE		Articles.DateDeleted IS NULL
								ORDER BY	Articles.DateCreated DESC";

			Articles = Connection.Query<Article>(SqlQuery, new { UrlSlug = UrlSlug, Language = Language }).ToList();
			ArticleTitle = Articles[0].Title;

			return Page();
		}
	}
}

