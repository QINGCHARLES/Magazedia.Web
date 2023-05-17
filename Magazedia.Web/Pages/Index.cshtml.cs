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
		private readonly string Language;

		//public IndexModel(ILogger<IndexModel> logger)

		public IndexModel(IConfiguration Config, IHttpContextAccessor HttpContextAccessor)
		{
			this.Config = Config;
			this.HttpContextAccessor = HttpContextAccessor;
			Language = Magazedia.Helpers.GetCultureFromHostname(HttpContextAccessor.HttpContext!.Request.Host.Host, "en");
			//_logger = logger;
		}



		public IList<Article>? Articles { get; set; }


		public IActionResult OnGet()
		{
			CultureInfo.CurrentCulture = new CultureInfo("ar");
			CultureInfo.CurrentUICulture = new CultureInfo("ar");
			using var Connection = new SqlConnection(Config.GetConnectionString("DefaultConnection"));

			string SqlQuery = "SELECT * FROM Articles WHERE Language = @Language ORDER BY DateCreated DESC";

			Articles = Connection.Query<Article>(SqlQuery, new { Language = Language }).ToList();


			return Page();
		}
	}
}
