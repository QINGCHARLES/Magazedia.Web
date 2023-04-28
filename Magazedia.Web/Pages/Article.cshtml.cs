using Markdig;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;
using Dapper;

namespace Magazedia.Web.Pages
{
    public class ArticleModel : PageModel
    {
        public string bean { get; set; }
        public string ArticleTitle { get; set; }
        public string ArticleText { get; set; }

        [BindProperty(SupportsGet = true)]
        public string? UrlSlug { get; set; }


        private readonly ILogger<ArticleModel> _logger;

        //public ArticleModel(ILogger<ArticleModel> logger)
        //{
        //    _logger = logger;
        //}

        public IActionResult OnGet()
        {
            using (var connection = new SqlConnection(@"TrustServerCertificate=True;Server=localhost,1433;Database=testdb;User Id=SA;Password=<YourStrong@Passw0rd>"))
            {

                var sql = "SELECT * FROM Article WHERE UrlSlug=@UrlSlug AND Language='en'"; ;


                var Article = connection.QuerySingleOrDefault(sql, new { UrlSlug = UrlSlug });

                if (Article is null)
                {
                    return NotFound();
                }

                var pipeline = new MarkdownPipelineBuilder().UseAdvancedExtensions().Build();

                // Console.WriteLine($"{product.ProductID} {product.ProductName}
                ArticleTitle = HttpContext.Request.Host.Host;
                //ArticleTitle = Article.Title;
                ArticleText = Markdown.ToHtml(Article.Text, pipeline);
            }

            return Page();
        }
    }
}

