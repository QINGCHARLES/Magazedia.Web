using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;
using Dapper;
using WikiWikiWorld.Models;

namespace Magazedia.Web.Pages;

public class TalkModel : PageModel
{
	public string? ArticleTitle { get; set; }

	[BindProperty(SupportsGet = true)]
	public string? UrlSlug { get; set; }

	public IList<ArticleTalkSubject>? ArticleTalkSubjects { get; set; }

	private readonly IConfiguration Config;
	private readonly string Language;
	public TalkModel(IConfiguration Config)
	{
		this.Config = Config;
		Language = "en";// Magazedia.Helpers.GetLanguage(HttpContext.Request.Host.Host);
	}

	public IActionResult OnGet()
	{
		using SqlConnection DbConnection = new( Config.GetConnectionString("DefaultConnection") );

		string? SqlQuery;

		SqlQuery = @"	SELECT		TOP(1) Title
						FROM		Articles
						WHERE		UrlSlug = @UrlSlug AND
									SiteId = @SiteId AND
									Language = @Language AND
									DateDeleted is null
						ORDER BY	DateCreated DESC";

		ArticleTitle = DbConnection.QuerySingle<string>(SqlQuery, new { UrlSlug, SiteId = 1, Language });

		//// ????
		SqlQuery = @"	SELECT		ArticleTalkSubjects.*, AspNetUsers.UserName AS CreatedByAspNetUsername
						FROM		ArticleTalkSubjects
						INNER JOIN	AspNetUsers ON ArticleTalkSubjects.CreatedByAspNetUserId = AspNetUsers.Id
						WHERE		ArticleTalkSubjects.ArticleTitle = @ArticleTitle AND
									ArticleTalkSubjects.SiteId = @SiteId AND
									ArticleTalkSubjects.Language = @Language AND
									ArticleTalkSubjects.DateDeleted IS NULL
						ORDER BY	ArticleTalkSubjects.DateCreated DESC";

		ArticleTalkSubjects = DbConnection.Query<ArticleTalkSubject>(SqlQuery, new { ArticleTitle, SiteId = 1, Language }).ToList();

		return Page();
	}
}
