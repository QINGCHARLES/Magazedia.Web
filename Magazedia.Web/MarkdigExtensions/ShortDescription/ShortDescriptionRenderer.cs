using Markdig.Helpers;
using Markdig.Renderers;
using Markdig.Renderers.Html;

namespace WikiWikiWorld.MarkdigExtensions;

public class ShortDescriptionRenderer : HtmlObjectRenderer<ShortDescription>
{
	private Magazedia.Web.Pages.BasePageModel Page;

	public ShortDescriptionRenderer(Magazedia.Web.Pages.BasePageModel Page)
	{
		this.Page = Page;
	}

	protected override void Write(HtmlRenderer renderer, ShortDescription obj)
	{
		StringSlice Data;

		Data = obj.Description;

		Page.MetaDescription = Data.ToString();
	}
}
