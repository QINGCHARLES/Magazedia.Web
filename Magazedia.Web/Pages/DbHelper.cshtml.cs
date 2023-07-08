using Dapper;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using Microsoft.IdentityModel.Tokens;
using WikiWikiWorld.Models;

namespace Magazedia.Web.Pages
{
    public class DbHelperModel : PageModel
    {
		public string? ArticleTitle { get; set; }
		public string? ArticleUrlSlug { get; set; }
		public string? ArticleText { get; set; }
		public string? ArticleMetaDesc { get; set; }
		public bool IsMagazine { get; set; }

		private readonly IConfiguration Config;
		private readonly string Language;
		public DbHelperModel(IConfiguration Config)
		{
			this.Config = Config;
			Language = "en";// Magazedia.Helpers.GetLanguage(HttpContext.Request.Host.Host);
		}

		public IActionResult OnPost()
		{
			ArticleTitle = Request.Form[nameof(ArticleTitle)].ToString().Replace("'", "''").Trim();
			ArticleText = Request.Form[nameof(ArticleText)].ToString().Replace("'", "''").Trim();
			ArticleUrlSlug = Request.Form[nameof(ArticleUrlSlug)].ToString().Trim();
			ArticleMetaDesc = RemoveQuotes(Request.Form[nameof(ArticleMetaDesc)].ToString().Replace("'", "''").Trim());

			

			//string helloo = Request.Form[nameof(IsMagazine)];
			IsMagazine = Request.Form[nameof(IsMagazine)].Contains("true");

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
				if(IsMagazine) ArticleTextB += "{{Categories Magazines}}";
				if (!String.IsNullOrWhiteSpace(ArticleMetaDesc)) ArticleTextB = "{{ShortDescription " + ArticleMetaDesc + "}}' + CHAR(13) + CHAR(10) + N'" + ArticleTextB;

				return Content(@$"	-- {ArticleTitle}
									INSERT Articles (Title, UrlSlug, SiteId, Culture)
									VALUES ( N'{ArticleTitle}', N'{ArticleUrlSlug}', 1, 'en' );
									INSERT ArticleRevisions (ArticleId, [Text], RevisionReason, CreatedByAspNetUserId)
									VALUES (SCOPE_IDENTITY(), N'{ArticleTextB}', N'Article created.', '7240be61-df81-46f9-8152-6a48b96abc40');

								".Replace("+ N'' ", "").Replace("\t", ""));
			}
		}

		public static string RemoveQuotes(string input)
		{
			if (string.IsNullOrEmpty(input))
				return input;

			char firstChar = input[0];
			char lastChar = input[input.Length - 1];

			if (firstChar == '"' || firstChar == '“' || firstChar == '”')
			{
				input = input.Substring(1);
			}

			if (lastChar == '"' || lastChar == '“' || lastChar == '”')
			{
				input = input.Substring(0, input.Length - 1);
			}

			return input;
		}

	}
}
