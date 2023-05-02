using Markdig;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;
using Dapper;
using WikiWikiWorld.Models;

namespace Magazedia.Web.Pages
{
    public class CreateModel : PageModel
    {
        public string ArticleTitle { get; set; }
        public string ArticleHtml { get; set; }
        public string ArticleText { get; set; }

        public IActionResult OnGet()
        {
            using (var connection = new SqlConnection(@"TrustServerCertificate=True;Server=localhost,1433;Database=testdb;User Id=SA;Password=<YourStrong@Passw0rd>"))
            {
//                var sql = "SELECT * FROM Article WHERE UrlSlug=@UrlSlug AND Language='en'";
  //              var Article = connection.QuerySingleOrDefault(sql, new { UrlSlug = UrlSlug });

            }

            return Page();
        }
    }
}

