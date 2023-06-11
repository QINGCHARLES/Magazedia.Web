using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Dapper;
using WikiWikiWorld.Models;

namespace Magazedia.Web.Pages;

public class TalkSubjectModel : BasePageModel
{
	public string? ArticleTitle { get; set; }
	public string? TalkSubject { get; set; }
	public IList<ArticleTalkSubjectPost>? ArticleTalkSubjectPosts { get; set; }

	[BindProperty(SupportsGet = true)]
	public string? ArticleUrlSlug { get; set; }

	[BindProperty(SupportsGet = true)]
	public string? ArticleTalkSubjectUrlSlug { get; set; }

	public TalkSubjectModel(IConfiguration Configuration, IHttpContextAccessor HttpContextAccessor) : base(Configuration, HttpContextAccessor) { }

	public IActionResult OnGet()
	{
		using SqlConnection DbConnection = new(Configuration.GetConnectionString("DefaultConnection"));

		string? SqlQuery;

		SqlQuery = @"	SELECT		ATP.*, A.Title AS ArticleTitle, ATS.Subject AS TalkSubject, AU.UserName AS CreatedByAspNetUsername
						FROM		ArticleTalkSubjectPosts ATP
						JOIN		ArticleTalkSubjects ATS ON ATP.ArticleTalkSubjectId = ATS.Id
						JOIN		Articles A ON ATS.ArticleId = A.Id
						JOIN		AspNetUsers AU ON ATP.CreatedByAspNetUserId = AU.Id
						WHERE		ATS.UrlSlug = @ArticleTalkSubjectUrlSlug AND
									A.UrlSlug = @ArticleUrlSlug
						ORDER BY	ATS.DateCreated ASC;
					";

		ArticleTalkSubjectPosts = DbConnection.Query<ArticleTalkSubjectPost>(SqlQuery, new { ArticleUrlSlug, ArticleTalkSubjectUrlSlug }).ToList();

		if (ArticleTalkSubjectPosts.Count > 0)
		{
			ArticleTitle = ArticleTalkSubjectPosts[0].ArticleTitle;
			TalkSubject = ArticleTalkSubjectPosts[0].TalkSubject;
		}
		else
		{
			// TODO
		}

		return Page();
	}
}
