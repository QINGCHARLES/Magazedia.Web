using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WikiWikiWorld.Models;
using Microsoft.Data.SqlClient;
using Dapper;

namespace Magazedia.Web.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;

private readonly IConfiguration Config;
private readonly string Language;

        //public IndexModel(ILogger<IndexModel> logger)

public IndexModel(IConfiguration Config)
        {
this.Config = Config;
Language = "en";
            //_logger = logger;
        }



public IList<Article>? Articles { get; set; }


        public IActionResult OnGet()
        {
using var Connection = new SqlConnection(Config.GetConnectionString("DefaultConnection"));

string SqlQuery = "SELECT * FROM Article WHERE Language = @Language ORDER BY DateCreated DESC";

Articles = Connection.Query<Article>(SqlQuery, new { Language = Language }).ToList();
        

return Page();
}
    }
}
