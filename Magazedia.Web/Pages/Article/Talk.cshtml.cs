using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Dapper;
using WikiWikiWorld.Models;
using System.Data;

namespace Magazedia.Web.Pages;

public class TalkModel : BasePageModel
{
	[BindProperty(SupportsGet = true)]
	public string? UrlSlug { get; set; }

	public WikiWikiWorld.Models.Article? Article { get; set; }
	public IList<ArticleTalkSubject>? ArticleTalkSubjects { get; set; }

	//public ArticleTalkSubject TalkSubject { get; set; }
	//public ArticleTalkSubjectPost TalkPost { get; set; }

	[BindProperty, Required, StringLength(300)]
	public string? Subject { get; set; }

	[BindProperty, Required, StringLength(2000)]
	public string? Text { get; set; }

	public TalkModel(IConfiguration Configuration, IHttpContextAccessor HttpContextAccessor) : base(Configuration, HttpContextAccessor) { }

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

		if(Article == null)
		{
			return Redirect($"/404:{UrlSlug}");
		}


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

	public IActionResult OnPost()
	{
		if (!ModelState.IsValid)
		{
			return Page(); // Return with validation errors
		}


		using SqlConnection Connection = new(Configuration.GetConnectionString("DefaultConnection"));
		{
			string SqlQuery = @"	SELECT		TOP(1) *
						FROM		Articles
						WHERE		UrlSlug = @UrlSlug AND
									SiteId = @SiteId AND
									Culture = @Culture AND
									DateDeleted IS NULL
					";

			Article = Connection.QuerySingle<WikiWikiWorld.Models.Article>(SqlQuery, new { UrlSlug, SiteId, Culture });

			int ArticleId = Article.Id;

			// Insert the new subject
			SqlQuery = @"	INSERT INTO ArticleTalkSubjects (SiteId, ArticleId, [Subject], UrlSlug, HasBeenEdited, CreatedByAspNetUserId)
							VALUES (@SiteId, @ArticleId, @Subject, @UrlSlug, @HasBeenEdited, @CreatedByAspNetUserId);
							SELECT CAST(SCOPE_IDENTITY() as int)";

			int ArticleTalkSubjectId = Connection.QuerySingle<int>(SqlQuery, new { SiteId, ArticleId, Subject, UrlSlug, HasBeenEdited = 0, CreatedByAspNetUserId = "7240be61-df81-46f9-8152-6a48b96abc40" });

			// Use the ID of the new subject for the post
			//TalkPost.ArticleTalkSubjectId = newSubjectId;

			SqlQuery = @"	INSERT INTO ArticleTalkSubjectPosts (ArticleTalkSubjectId, ParentTalkSubjectPostId, [Text], HasBeenEdited, CreatedByAspNetUserId)
							VALUES (@ArticleTalkSubjectId, NULL, @Text, @HasBeenEdited, @CreatedByAspNetUserId)";

			Connection.Execute(SqlQuery, new { SiteId, ArticleTalkSubjectId, Text, HasBeenEdited = 0, CreatedByAspNetUserId = "7240be61-df81-46f9-8152-6a48b96abc40" });
		}

		return Page();
	}
}
