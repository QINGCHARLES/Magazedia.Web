using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;
using Dapper;
using WikiWikiWorld.Models;

namespace Magazedia.Web.Pages
{
	public class HistoryModel : BasePageModel
	{
		public string? ArticleTitle { get; set; }

		[BindProperty(SupportsGet = true)]
		public string? UrlSlug { get; set; }

		public IList<WikiWikiWorld.Models.ArticleRevision>? ArticleRevisions { get; set; }

		public HistoryModel(IConfiguration Configuration, IHttpContextAccessor HttpContextAccessor) : base(Configuration, HttpContextAccessor) { }

		public IActionResult OnGet()
		{
			using var Connection = new SqlConnection(Configuration.GetConnectionString("DefaultConnection"));

			// Get a list of all the revisions of this Article and convert the UserId in the Article table to a Username for display
			string SqlQuery = @"SELECT		ArticleRevisions.Id, ArticleRevisions.ArticleId, Articles.Title, Articles.UrlSlug, ArticleRevisions.[Text], ArticleRevisions.RevisionReason, AspNetUsers.Id as CreatedByAspNetUserId, AspNetUsers.UserName as CreatedByAspNetUsername, ArticleRevisions.DateCreated, ArticleRevisions.DateDeleted
								FROM		Articles
								INNER JOIN	ArticleRevisions ON Articles.Id = ArticleRevisions.ArticleId
								INNER JOIN	AspNetUsers ON ArticleRevisions.CreatedByAspNetUserId = AspNetUsers.Id
								WHERE		Articles.UrlSlug = @UrlSlug AND
											Articles.SiteId = @SiteId AND
											Articles.Culture = @Culture AND
											Articles.DateDeleted IS NULL AND
											ArticleRevisions.DateDeleted IS NULL
								ORDER BY ArticleRevisions.DateCreated DESC
								";

			ArticleRevisions = Connection.Query<WikiWikiWorld.Models.ArticleRevision>(SqlQuery, new { UrlSlug, SiteId, Culture }).ToList();
			ArticleTitle = ArticleRevisions[0].Title;

			return Page();
		}
	}
}

