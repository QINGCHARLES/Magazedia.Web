using Markdig;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;
using Dapper;
using WikiWikiWorld.Models;

namespace Magazedia.Web.Pages
{
    public class EditModel : PageModel
    {
        public string ArticleTitle { get; set; }
        public string ArticleUrlSlug { get; set; }

        public string ArticleHtml { get; set; }
        public string ArticleRevisionReason { get; set; }
        public string ArticleText { get; set; }

        [BindProperty(SupportsGet = true)]
        public string? UrlSlug { get; set; }

//private readonly IConfiguration Configuration;

	public IActionResult OnPost()
	{
		ArticleText = Request.Form[nameof(ArticleText)];
		ArticleRevisionReason = Request.Form[nameof(ArticleRevisionReason)];

            using var Connection = new SqlConnection(@"TrustServerCertificate=True;Server=localhost,1433;Database=Magazedia;User Id=SA;Password=<YourStrong@Passw0rd>");
                var sql = "SELECT TOP(1) * FROM Article WHERE UrlSlug=@UrlSlug AND Language='en' ORDER BY DateCreated DESC";
                var Article = Connection.QuerySingleOrDefault(sql, new { UrlSlug = UrlSlug });

		string SqlQuery = "INSERT Article (Title, UrlSlug, [Text], RevisionReason, CreatedByAspNetUserId, SiteId, Language) VALUES (@Title, @UrlSlug, @Text, @RevisionReason, @CreatedByAspNetUserId, @SiteId, @Language);";
                var res = Connection.Execute( SqlQuery, new { Title = Article.Title, UrlSlug = Article.UrlSlug, Text = ArticleText, RevisionReason = ArticleRevisionReason, CreatedByAspNetUserId = "blehuser", SiteId = 1, Language = "en" });

          

            return Content("output:" + res.ToString() + UrlSlug + ArticleText);
	}

        public IActionResult OnGet()
        {
            using (var connection = new SqlConnection(@"TrustServerCertificate=True;Server=localhost,1433;Database=Magazedia;User Id=SA;Password=<YourStrong@Passw0rd>"))
            {
                var sql = "SELECT TOP(1) * FROM Article WHERE UrlSlug=@UrlSlug AND Language='en' ORDER BY DateCreated DESC";
                var Article = connection.QuerySingleOrDefault(sql, new { UrlSlug = UrlSlug });

                if (Article is null)
                {
                    return NotFound();
                }

                var pipeline = new MarkdownPipelineBuilder().UseAdvancedExtensions().Build();

                ArticleTitle = Article.Title;
                ArticleUrlSlug = Article.UrlSlug;
                ArticleText = Article.Text;
                ArticleHtml = Markdown.ToHtml(Article.Text, pipeline);
            }

            return Page();
        }
    }
}

