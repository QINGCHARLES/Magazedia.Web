using Dapper;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using WikiWikiWorld.Models;

namespace Magazedia.Web.Pages
{
    public class DbHelperModel : PageModel
    {
		public string? ArticleTitle { get; set; }
		public string? ArticleUrlSlug { get; set; }
		public string? ArticleText { get; set; }

		private readonly IConfiguration Config;
		private readonly string Language;
		public DbHelperModel(IConfiguration Config)
		{
			this.Config = Config;
			Language = "en";// Magazedia.Helpers.GetLanguage(HttpContext.Request.Host.Host);
		}

		public IActionResult OnPost()
        {
			ArticleTitle = Request.Form[nameof(ArticleTitle)].ToString().Replace("'", "''");
			ArticleText = Request.Form[nameof(ArticleText)].ToString().Replace("'", "''");
			ArticleUrlSlug = Request.Form[nameof(ArticleUrlSlug)];

			if(ArticleTitle=="x" && ArticleUrlSlug == "")
			{
				using var Connection = new SqlConnection(Config!.GetConnectionString("DefaultConnection"));

				string SqlQuery = Request.Form[nameof(ArticleText)].ToString();
				var result = Connection.Execute(SqlQuery);

				return Content("output: " + result);
			}
			else
			{
				string ArticleTextB = ArticleText.Replace("\r\n", "' + CHAR(13) + CHAR(10) + N'");

				return Content(@$"	INSERT Articles (Title, UrlSlug, SiteId, Culture)
									VALUES ( N'{ArticleTitle}', N'{ArticleUrlSlug}', 1, 'en' );
									INSERT ArticleRevisions (ArticleId, [Text], RevisionReason, CreatedByAspNetUserId)
									VALUES (SCOPE_IDENTITY(), N'{ArticleTextB}', N'Article created.', '7240be61-df81-46f9-8152-6a48b96abc40');

								".Replace("+ N'' ", "").Replace("\t", ""));
			}
		}
    }
}
