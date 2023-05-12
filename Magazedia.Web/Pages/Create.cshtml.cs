using Markdig;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;
using Dapper;
using WikiWikiWorld.Models;
using System.Security.Claims;

namespace Magazedia.Web.Pages;
public class CreateModel : PageModel
{
    public string ArticleTitle { get; set; }
    public string ArticleText { get; set; }

private readonly IConfiguration Config;
private readonly IHttpContextAccessor HttpContextAccessor;
private readonly string Language;

public CreateModel(IConfiguration Config, IHttpContextAccessor HttpContextAccessor)
{
this.Config = Config;
this.HttpContextAccessor = HttpContextAccessor;
Language = "en";
}


    public IActionResult OnPost()
    {
 ArticleText = Request.Form[nameof(ArticleText)];
 ArticleTitle = Request.Form[nameof(ArticleTitle)];


ClaimsPrincipal? user = HttpContextAccessor.HttpContext?.User;
string Username = user?.Identity?.Name ?? "Anonymous";
using var Connection = new SqlConnection(Config.GetConnectionString("DefaultConnection"));
var SlugOptions = new UnicodeSlug.SlugOptions();
string UrlSlug = SlugOptions.GenerateSlug(ArticleTitle);
string ArticleRevisionReason = "Created";
var SqlQuery = "INSERT Article (Title, UrlSlug, [Text], RevisionReason, CreatedByAspNetUserId, SiteId, Language) VALUES (@Title, @UrlSlug, @Text, @RevisionReason, @CreatedByAspNetUserId, @SiteId, @Language);";
var res = Connection.Execute( SqlQuery, new { Title = ArticleTitle, UrlSlug = UrlSlug, Text = ArticleText, RevisionReason = ArticleRevisionReason, CreatedByAspNetUserId = Username, SiteId = 1, Language = Language });
return Content("output:" + res.ToString() + UrlSlug + ArticleText);

//                var sql = "SELECT * FROM Article WHERE UrlSlug=@UrlSlug AND Language='en'";
//              var Article = connection.QuerySingleOrDefault(sql, new { UrlSlug = UrlSlug });

        

        return Page();
    }
}

