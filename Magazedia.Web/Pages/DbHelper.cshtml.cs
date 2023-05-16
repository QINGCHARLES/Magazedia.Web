using Dapper;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;
using WikiWikiWorld.Models;

namespace Magazedia.Web.Pages
{
    public class DbHelperModel : PageModel
    {
		public string? ArticleTitle { get; set; }
		public string? ArticleUrlSlug { get; set; }
		public string? ArticleText { get; set; }

		[Inject]
		private IConfiguration? Config { get; set; }

		public IActionResult OnPost()
        {
			ArticleTitle = Request.Form[nameof(ArticleTitle)].ToString().Replace("'", "''");
			ArticleText = Request.Form[nameof(ArticleText)].ToString().Replace("'", "''");
			ArticleUrlSlug = Request.Form[nameof(ArticleUrlSlug)];

			if(ArticleTitle=="x" && ArticleUrlSlug == "")
			{
				using var Connection = new SqlConnection(Config!.GetConnectionString("DefaultConnection"));

				string SqlQuery = ArticleText;
				var result = Connection.Execute(SqlQuery);

				return Content("output: " + result);
			}
			else
			{

				return Content(@$"	INSERT Articles ( Title, UrlSlug, [Text], RevisionReason, CreatedByAspNetUserId, SiteId, Language )
									VALUES ( N'{ArticleTitle}', N'{ArticleUrlSlug}', N'{ArticleText}', N'Article created', '7240be61-df81-46f9-8152-6a48b96abc40', 1, 'en' );".Replace(")\r\n", ") ").Replace("\r\n", "' + CHAR(13) + CHAR(10) + N'").Replace("+ '' ", "").Replace("\t", ""));
			}
		}
    }
}
