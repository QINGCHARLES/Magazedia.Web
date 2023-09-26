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
public class CreateModel : BasePageModel
{
	public string? ArticleTitle { get; set; }
	public string? ArticleText { get; set; }

	public CreateModel(IConfiguration Configuration, IHttpContextAccessor HttpContextAccessor) : base(Configuration, HttpContextAccessor) { }

	public IActionResult OnPost()
	{
		var specificUsername = "QINGCHARLES";

		if (this.User == null || this.User.Identity == null || !this.User.Identity.IsAuthenticated || (this.User.Identity.IsAuthenticated && this.User.Identity.Name != specificUsername))
		{

			return BadRequest();
		}

		ArticleText = Request.Form[nameof(ArticleText)];
		ArticleTitle = Request.Form[nameof(ArticleTitle)];

		int SiteId = 1;

		ClaimsPrincipal? User = HttpContextAccessor.HttpContext?.User;
		string Username = User?.FindFirstValue(ClaimTypes.NameIdentifier) ?? "7240be61-df81-46f9-8152-6a48b96abc40";// "Anonymous";

		using var Connection = new SqlConnection(Configuration.GetConnectionString("DefaultConnection"));

		var SlugOptions = new UnicodeSlug.SlugOptions();
		string UrlSlug = SlugOptions.GenerateSlug(ArticleTitle);
		//string ArticleRevisionReason = "Created";
		//var SqlQuery = "INSERT Articles (Title, UrlSlug, [Text], RevisionReason, CreatedByAspNetUserId, SiteId, Language) VALUES (@Title, @UrlSlug, @Text, @RevisionReason, @CreatedByAspNetUserId, @SiteId, @Language);";
		//var res = Connection.Execute(SqlQuery, new { Title = ArticleTitle, UrlSlug = UrlSlug, Text = ArticleText, RevisionReason = ArticleRevisionReason, CreatedByAspNetUserId = Username, SiteId = 1, Language = Language });
		//return Content("output:" + res.ToString() + UrlSlug + ArticleText);

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

