using Markdig.Helpers;
using Markdig.Renderers;
using Markdig.Renderers.Html;

namespace WikiWikiWorld.MarkdigExtensions;

public class ImageRenderer : HtmlObjectRenderer<Image>
{
	private int SiteId;

	public ImageRenderer(int SiteId)
	{
		this.SiteId = SiteId;
	}

	protected override void Write(HtmlRenderer renderer, Image obj)
	{
		StringSlice Data;

		Data = obj.Data;

		if (renderer.EnableHtmlForInline)
		{
			// write a full a tag
			renderer.Write($"<img style=\"float:left;\" src=\"/sitefiles/{SiteId}/{Data.ToString().Split("file:", 2)[1]}\">");

		}
		else
		{
			// inline HTML is disabled, so write the raw value
			renderer.Write('#').Write(obj.Data);
		}
	}
}
