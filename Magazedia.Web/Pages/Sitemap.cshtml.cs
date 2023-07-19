using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text;

namespace Magazedia.Web.Pages
{
	public class SitemapModel : PageModel
	{
		public IActionResult OnGet()
		{
			Response.ContentType = "application/xml";
			
			var xml = @"<?xml version=""1.0"" encoding=""UTF-8""?>
							<urlset xmlns=""http://www.sitemaps.org/schemas/sitemap/0.9"">
								<url>
									<loc>https://magazedia.wiki/</loc>
								</url>
							</urlset>";

			//<lastmod>2023-07-18</lastmod>
			//<changefreq>weekly</changefreq>
			//<priority>0.8</priority>

			return Content(xml, "application/xml", Encoding.UTF8);
		}

	}
}
