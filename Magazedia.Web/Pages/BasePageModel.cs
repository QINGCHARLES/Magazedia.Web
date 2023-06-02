using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Magazedia.Web.Pages;

public abstract class BasePageModel : PageModel
{
	protected readonly IConfiguration Configuration;
	protected readonly IHttpContextAccessor HttpContextAccessor;

	public string Culture { get; }

	protected BasePageModel(IConfiguration Configuration, IHttpContextAccessor HttpContextAccessor)
	{
		this.Configuration = Configuration;
		this.HttpContextAccessor = HttpContextAccessor;

		// Set Culture in the constructor
		Culture = Magazedia.Helpers.GetCultureFromHostname(HttpContextAccessor.HttpContext!.Request.Host.Host, "en");
	}
}

