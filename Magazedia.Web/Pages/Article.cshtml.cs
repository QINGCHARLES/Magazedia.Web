using Markdig;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;
using Dapper;

namespace Magazedia.Web.Pages
{
    public class ArticleModel : PageModel
    {
        public string? ArticleTitle { get; set; }
        public string? ArticleText { get; set; }

        [BindProperty(SupportsGet = true)]
        public string? UrlSlug { get; set; }

		private readonly IConfiguration _config;

		public ArticleModel(IConfiguration config)
		{
			_config = config;
		}

		public IActionResult OnGet()
        {
			string? connectionString = _config.GetConnectionString("DefaultConnection");

			using (var connection = new SqlConnection(connectionString))
            {

                var sql = "SELECT TOP(1) * FROM Article WHERE UrlSlug = @UrlSlug AND Language = 'en' AND DateDeleted IS NULL ORDER BY DateCreated DESC";


                var Article = connection.QuerySingleOrDefault(sql, new { UrlSlug = UrlSlug });

                if (Article is null)
                {
                    return NotFound();
                }

                var pipeline = new MarkdownPipelineBuilder().UseAdvancedExtensions().Build();

                ArticleTitle = HttpContext.Request.Host.Host;
                ArticleTitle = Article.Title;
                ArticleText = Markdown.ToHtml(Article.Text, pipeline);
            }

            return Page();
        }
    }
}

