using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Dapper;
using WikiWikiWorld.Models;

namespace Magazedia.Web.Pages;

public class TalkModel : BasePageModel
{
	[BindProperty(SupportsGet = true)]
	public string? UrlSlug { get; set; }

	public WikiWikiWorld.Models.Article? Article { get; set; }
	public IList<ArticleTalkSubject>? ArticleTalkSubjects { get; set; }

	public TalkModel(IConfiguration Configuration, IHttpContextAccessor HttpContextAccessor) : base(Configuration, HttpContextAccessor) { }

	public TalkModel(IConfiguration Configuration, )

	public IActionResult OnGet()
	{
		using SqlConnection Connection = new(Configuration.GetConnectionString("DefaultConnection"));

		string? SqlQuery;

		SqlQuery = @"	SELECT		TOP(1) *
						FROM		Articles
						WHERE		UrlSlug = @UrlSlug AND
									SiteId = @SiteId AND
									Culture = @Culture AND
									DateDeleted IS NULL
					";

		Article = Connection.QuerySingle<WikiWikiWorld.Models.Article>(SqlQuery, new { UrlSlug, SiteId, Culture });

		// Find and display any Talk subjects for this Article
		SqlQuery = @"	
						SELECT		*
						FROM		ArticleTalkSubjects
						WHERE		ArticleId = @ArticleId AND
									SiteId = @SiteId AND
									DateDeleted IS NULL
						ORDER BY	DateCreated ASC;
					";
		ArticleTalkSubjects = Connection.Query<WikiWikiWorld.Models.ArticleTalkSubject>(SqlQuery, new { SiteId, ArticleId = Article.Id }).ToList();

		return Page();
	}
}
