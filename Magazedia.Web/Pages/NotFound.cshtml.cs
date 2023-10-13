using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Magazedia.Web.Pages
{
    public class NotFoundModel : PageModel
    {
        public IActionResult OnGet()
        {
        
			this.Response.StatusCode = StatusCodes.Status404NotFound;
            return Page();
        }
    }
}
