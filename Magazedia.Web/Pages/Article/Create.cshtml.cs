using Markdig;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;
using Dapper;
using WikiWikiWorld.Models;
using System.Security.Claims;
using System.Security.Policy;
using System.Text.RegularExpressions;

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
		var SqlQuery = "INSERT Articles (Title, UrlSlug, [Text], RevisionReason, CreatedByAspNetUserId, SiteId, Language) VALUES (@Title, @UrlSlug, @Text, @RevisionReason, @CreatedByAspNetUserId, @SiteId, @Language);";
		var res = Connection.Execute(SqlQuery, new { Title = ArticleTitle, UrlSlug = UrlSlug, Text = ArticleText, RevisionReason = ArticleRevisionReason, CreatedByAspNetUserId = Username, SiteId = 1, Language = Language });
		return Content("output:" + res.ToString() + UrlSlug + ArticleText);

		//                var sql = "SELECT * FROM Article WHERE UrlSlug=@UrlSlug AND Language='en'";
		//              var Article = connection.QuerySingleOrDefault(sql, new { UrlSlug = UrlSlug });

		//Connection.Open();
		//string resout = "";
		//using (var tran = Connection.BeginTransaction())
		//{
		//	// Execute your queries here
		//	string SqlQuery = @"INSERT Articles (Title, UrlSlug, SiteId, Culture)
		//						OUTPUT INSERTED.[Id]
		//						VALUES (@Title, @UrlSlug, @SiteId, @Culture);
		//						"
		//	;
		//	int ArticleId = Connection.QuerySingle<int>(SqlQuery, new { Title = ArticleTitle, UrlSlug = ArticleUrlSlug, SiteId, Culture }, tran);

		//	SqlQuery = @"	INSERT ArticleRevisions (ArticleId, [Text], RevisionReason, CreatedByAspNetUserId)
		//					VALUES (@ArticleId, @Text, @RevisionReason, @CreatedByAspNetUserId);
		//				";
		//	var res = Connection.Execute(SqlQuery, new { ArticleId, Text = ArticleText, RevisionReason = ArticleRevisionReason, CreatedByAspNetUserId = Username }, tran);
		//	resout = res.ToString();
		//	tran.Commit(); //Or rollback 
		//}

		return Page();
	}
}

