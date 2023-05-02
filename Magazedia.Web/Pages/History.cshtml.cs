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
        public string ArticleTitle { get; set; }

        [BindProperty(SupportsGet = true)]
        public string? UrlSlug { get; set; }

	public IList<Article>? Articles { get; set; }

        public IActionResult OnGet()
        {
            using (var connection = new SqlConnection(@"TrustServerCertificate=True;Server=localhost,1433;Database=Magazedia;User Id=SA;Password=<YourStrong@Passw0rd>"))
            {
                var sql = "SELECT * FROM Article WHERE UrlSlug=@UrlSlug AND Language='en' ORDER BY DateCreated DESC";
                var ArticlesList = connection.Query<Article>(sql, new { UrlSlug = UrlSlug }).ToList();
Articles = ArticlesList;
                ArticleTitle = ArticlesList[0].Title;
            }

            return Page();
        }
    }
}

