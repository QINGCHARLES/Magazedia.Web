using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;
using Dapper;
using WikiWikiWorld.Models;

namespace Magazedia.Web.Pages;

public class TalkSubjectModel : PageModel
{
	public string? ArticleTitle { get; set; }

	[BindProperty(SupportsGet = true)]
	public string? ArticleUrlSlug { get; set; }

	[BindProperty(SupportsGet = true)]
	public string? ArticleTalkSubjectUrlSlug { get; set; }
	public IList<ArticleTalkSubjectPost>? ArticleTalkSubjectPosts { get; set; }

	private readonly IConfiguration Config;
	private readonly string Language;
	public TalkSubjectModel(IConfiguration Config)
	{
		this.Config = Config;
		Language = "en";// Magazedia.Helpers.GetLanguage(HttpContext.Request.Host.Host);
	}

	public IActionResult OnGet()
	{
		//using var Connection = new SqlConnection(Config.GetConnectionString("DefaultConnection"));

		//// ????
		//string SqlQuery = @"SELECT		ArticleTalkSubjectPosts.*, AspNetUsers.UserName AS CreatedByAspNetUsername
		//						FROM		ArticleTalkSubjectPosts
		//						INNER JOIN	AspNetUsers ON ArticleTalkSubjectPosts.CreatedByAspNetUserId = AspNetUsers.Id
		//						WHERE		ArticleTalkSubjectPosts.ArticleTalkSubjectId = 1 AND
		//									ArticleTalkSubjectPosts.DateDeleted IS NULL
		//						ORDER BY	ArticleTalkSubjectPosts.DateCreated DESC";

		//ArticleTitle = "GQ (USA) - November 2020";

		//ArticleTalkSubjectPosts = Connection.Query<ArticleTalkSubject>(SqlQuery, new { ArticleTitle = ArticleTitle, SiteId = 1, Language = Language }).ToList();

		//return Page();

		using SqlConnection DbConnection = new(Config.GetConnectionString("DefaultConnection"));

		string? SqlQuery;

		SqlQuery = @"	SELECT		TOP(1) Title
						FROM		Articles
						WHERE		UrlSlug = @ArticleUrlSlug AND
									SiteId = @SiteId AND
									Language = @Language AND
									DateDeleted IS NULL
						ORDER BY	DateCreated DESC";

		ArticleTitle = DbConnection.QuerySingle<string>(SqlQuery, new { ArticleUrlSlug, SiteId = 1, Language });

		//// ????
		SqlQuery = @"	SELECT		Id
						FROM		ArticleTalkSubjects
						WHERE		ArticleTitle = @ArticleTitle AND
									UrlSlug = @ArticleTalkSubjectUrlSlug AND
									SiteId = @SiteId AND
									Language = @Language AND
									DateDeleted IS NULL";

		int ArticleTalkSubjectId = DbConnection.QuerySingle<int>(SqlQuery, new { ArticleTitle, ArticleTalkSubjectUrlSlug, SiteId = 1, Language });

		SqlQuery = @"	SELECT		ArticleTalkSubjectPosts.*, AspNetUsers.UserName AS CreatedByAspNetUsername
						FROM		ArticleTalkSubjectPosts
						INNER JOIN	AspNetUsers ON ArticleTalkSubjectPosts.CreatedByAspNetUserId = AspNetUsers.Id
						WHERE		ArticleTalkSubjectPosts.ArticleTalkSubjectId = @ArticleTalkSubjectId AND
									ArticleTalkSubjectPosts.DateDeleted IS NULL
						ORDER BY	ArticleTalkSubjectPosts.DateCreated ASC";

		ArticleTalkSubjectPosts = DbConnection.Query<ArticleTalkSubjectPost>(SqlQuery, new { ArticleTalkSubjectId }).ToList();

		//return Content($"output: {ArticleUrlSlug}###{ArticleTalkSubjectUrlSlug}");// --- {ArticleUrlSlug} --- {ArticleTalkSubjectUrlSlug}");
		return Page();
	}
}
