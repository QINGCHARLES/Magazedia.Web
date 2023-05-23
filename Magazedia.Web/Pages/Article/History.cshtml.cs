using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;
using Dapper;
using WikiWikiWorld.Models;

namespace Magazedia.Web.Pages
{
	public class HistoryModel : PageModel
	{
		public string? ArticleTitle { get; set; }

		[BindProperty(SupportsGet = true)]
		public string? UrlSlug { get; set; }

		public IList<WikiWikiWorld.Models.ArticleRevision>? ArticleRevisions { get; set; }

		private readonly IConfiguration Config;
		private readonly string Culture;
		public HistoryModel(IConfiguration Config, IHttpContextAccessor HttpContextAccessor)
		{
			this.Config = Config;
			Culture = Magazedia.Helpers.GetCultureFromHostname(HttpContextAccessor.HttpContext!.Request.Host.Host, "en");
		}

		public IActionResult OnGet()
		{
			using var Connection = new SqlConnection(Config.GetConnectionString("DefaultConnection"));

			int SiteId = 1;

			// Get a list of all the revisions of this Article and convert the UserId in the Article table to a Username for display
			string SqlQuery = @"SELECT		ar.Id, a.Title, a.UrlSlug, ar.[Text], ar.RevisionReason, ar.DateCreated, u.UserName as CreatorUsername
								FROM		Articles a
								INNER JOIN	ArticleRevisions ar ON a.Id = ar.ArticleId
								INNER JOIN	AspNetUsers u ON ar.CreatedByAspNetUserId = u.Id
								WHERE		a.UrlSlug = @UrlSlug AND
											a.SiteId = @SiteId AND
											a.Culture = @Culture AND
											a.DateDeleted IS NULL AND
											ar.DateDeleted IS NULL
								ORDER BY ar.DateCreated DESC
								";

			ArticleRevisions = Connection.Query<WikiWikiWorld.Models.ArticleRevision>(SqlQuery, new { UrlSlug, SiteId, Culture }).ToList();
			ArticleTitle = ArticleRevisions[0].Title;

			return Page();
		}
	}
}

