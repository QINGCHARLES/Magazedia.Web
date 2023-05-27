using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WikiWikiWorld.Models;
using Microsoft.Data.SqlClient;
using Dapper;
using System.Globalization;

namespace Magazedia.Web.Pages
{
	public class IndexModel : PageModel
	{
		//private readonly ILogger<IndexModel> _logger;

		private readonly IConfiguration Config;
		private readonly IHttpContextAccessor HttpContextAccessor;
		private readonly string Culture;

		//public IndexModel(ILogger<IndexModel> logger)

		public IndexModel(IConfiguration Config, IHttpContextAccessor HttpContextAccessor)
		{
			this.Config = Config;
			this.HttpContextAccessor = HttpContextAccessor;
			Culture = Magazedia.Helpers.GetCultureFromHostname(HttpContextAccessor.HttpContext!.Request.Host.Host, "en");
			//_logger = logger;
		}



		public IEnumerable<WikiWikiWorld.Models.Article>? Articles { get; set; }


		public IActionResult OnGet()
		{
			using var Connection = new SqlConnection(Config.GetConnectionString("DefaultConnection"));
			int SiteId = 1;
			string SqlQuery = @"SELECT		*
								FROM		Articles
								WHERE		SiteId = @SiteId AND
											Culture = @Culture AND
											DateDeleted IS NULL
								ORDER BY	DateCreated DESC
								";

			Articles = Connection.Query<WikiWikiWorld.Models.Article>(SqlQuery, new { SiteId, Culture });

			return Page();
		}
	}
}
