using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Magazedia.Web.Pages
{
    public class CreateEditModel : PageModel
    {

    [BindProperty(SupportsGet = true)]
        public string? UrlSlug { get; set; }

        public void OnGet()
        {

ViewData["slug"] = UrlSlug;
        }
    }
}
