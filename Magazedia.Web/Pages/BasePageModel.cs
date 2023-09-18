using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Magazedia.Web.Pages;

public abstract class BasePageModel : PageModel
{
	protected readonly IConfiguration Configuration;
	protected readonly IHttpContextAccessor HttpContextAccessor;

	public string Culture { get; }
	public int SiteId { get; }
	public string? MetaDescription { get; set; }
	public bool AllowSearchEngineIndexing { get; set; }

	protected BasePageModel(IConfiguration Configuration, IHttpContextAccessor HttpContextAccessor)
	{
		this.Configuration = Configuration;
		this.HttpContextAccessor = HttpContextAccessor;

		Culture = Magazedia.Helpers.GetCultureFromHostname(HttpContextAccessor.HttpContext!.Request.Host.Host, "en");
		SiteId = 1;
		AllowSearchEngineIndexing = true;
	}
}

