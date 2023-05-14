using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Magazedia.Web.Pages
{
    public class DbHelperModel : PageModel
    {
		public string? ArticleTitle { get; set; }
		public string? ArticleUrlSlug { get; set; }
		public string? ArticleText { get; set; }

		public IActionResult OnPost()
        {
			ArticleTitle = Request.Form[nameof(ArticleTitle)].ToString().Replace("'", "''");
			ArticleText = Request.Form[nameof(ArticleText)].ToString().Replace("'", "''");
			ArticleUrlSlug = Request.Form[nameof(ArticleUrlSlug)];

			return Content(@$"	INSERT Articles ( Title, UrlSlug, [Text], RevisionReason, CreatedByAspNetUserId, SiteId, Language )
								VALUES ( N'{ArticleTitle}', N'{ArticleUrlSlug}', N'{ArticleText}', N'Article created', '7240be61-df81-46f9-8152-6a48b96abc40', 1, 'en' );".Replace(")\r\n", ") ").Replace("\r\n", "' + CHAR(13) + CHAR(10) + N'").Replace("+ '' ", "").Replace("\t", ""));
		}
    }
}
