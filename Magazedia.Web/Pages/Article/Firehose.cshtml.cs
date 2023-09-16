using Markdig;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;
using Dapper;
using WikiWikiWorld.Models;
using System.Security.Policy;
using System.Text.RegularExpressions;

namespace Magazedia.Web.Pages
{
	public class FirehoseModel : BasePageModel
	{
		public string? ArticleTitle { get; set; }


		public IEnumerable<WikiWikiWorld.Models.ArticleRevision>? ArticleRevisions { get; set; }

		public FirehoseModel(IConfiguration Configuration, IHttpContextAccessor HttpContextAccessor) : base(Configuration, HttpContextAccessor) { }

		public IActionResult OnGet()
		{
			using var Connection = new SqlConnection(Configuration.GetConnectionString("DefaultConnection"));

            string SqlQuery = @"SELECT		ar.Id, ar.ArticleId, a.Title, a.UrlSlug, ar.[Text], ar.RevisionReason, ar.CreatedByAspNetUserId, u.UserName as CreatedByAspNetUsername, ar.DateCreated, ar.DateDeleted
								FROM		Articles a
								INNER JOIN	ArticleRevisions ar ON a.Id = ar.ArticleId
								INNER JOIN	AspNetUsers u ON ar.CreatedByAspNetUserId = u.Id
								WHERE		a.SiteId = @SiteId AND
											a.Culture = @Culture AND
											a.DateDeleted IS NULL AND
											ar.DateDeleted IS NULL
								ORDER BY ar.DateCreated DESC
								";

			ArticleRevisions = Connection.Query<WikiWikiWorld.Models.ArticleRevision>(SqlQuery, new { SiteId, Culture });

			return Page();
		}

		public IActionResult OnPostDelete(string Id)
		{
			using var Connection = new SqlConnection(Configuration.GetConnectionString("DefaultConnection"));

			// Delete the Article revision. If it was the only remaining revision, then delete the Article row too.
			string SqlQuery = @"UPDATE	ArticleRevisions SET DateDeleted = GETDATE() WHERE Id = @Id;
								DECLARE	@ArticleIdToDelete int;
								SET		@ArticleIdToDelete = (SELECT ArticleId FROM ArticleRevisions WHERE Id = @Id);
								IF NOT EXISTS (SELECT 1 FROM ArticleRevisions WHERE ArticleId = @ArticleIdToDelete AND DateDeleted IS NULL)
								BEGIN
									UPDATE Articles SET DateDeleted = GETDATE() WHERE Id = @ArticleIdToDelete;
								END
								";

			Connection.Execute(SqlQuery, new { Id });

			return LocalRedirect("/firehose:");
		}
	}
}

