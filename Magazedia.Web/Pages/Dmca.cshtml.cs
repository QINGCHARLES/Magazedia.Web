using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Magazedia.Web.Pages
{
    public class DmcaModel : PageModel
    {
		private readonly IConfiguration Config;
		private readonly string Language;
		public DmcaModel(IConfiguration Config)
		{
			this.Config = Config;
			Language = "en";// Magazedia.Helpers.GetLanguage(HttpContext.Request.Host.Host);
		}

		public void OnGet()
        {
        }
    }
}
