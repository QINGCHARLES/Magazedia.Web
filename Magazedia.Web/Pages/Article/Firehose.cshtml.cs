using Markdig;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;
using Dapper;
using WikiWikiWorld.Models;

namespace Magazedia.Web.Pages
{
	public class FirehoseModel : PageModel
	{
		public string? ArticleTitle { get; set; }


		public IList<WikiWikiWorld.Models.Article>? Articles { get; set; }

		private readonly IConfiguration Config;
		private readonly string Language;
		public FirehoseModel(IConfiguration Config)
		{
			this.Config = Config;
			Language = "en";// Magazedia.Helpers.GetLanguage(HttpContext.Request.Host.Host);
		}

		public IActionResult OnGet()
		{
			using var Connection = new SqlConnection(Config.GetConnectionString("DefaultConnection"));

			string SqlQuery = "SELECT * FROM Articles WHERE Language = @Language ORDER BY DateCreated DESC";
			Articles = Connection.Query<WikiWikiWorld.Models.Article>(SqlQuery, new { Language = Language }).ToList();
			//Articles = ArticlesList;
//			ArticleTitle = Articles[0].Title;

			return Page();
		}

		public IActionResult OnPostDelete(string Id)
		{
			using var Connection = new SqlConnection(Config.GetConnectionString("DefaultConnection"));

			//Articles = ArticlesList;
//			ArticleTitle = Articles[0].Title;

string SqlQuery = "DELETE Articles WHERE Id = @Id;";
var res = Connection.Execute( SqlQuery, new { Id = Id });

	//		return Content(res.ToString());;
	return LocalRedirect("/firehose:");
		}
	}
}

