using Markdig.Helpers;
using Markdig.Renderers;
using Markdig.Renderers.Html;

namespace WikiWikiWorld.MarkdigExtensions;

public class ImageRenderer : HtmlObjectRenderer<Image>
{
	protected override void Write(HtmlRenderer renderer, Image obj)
	{
		StringSlice issueNumber;

		issueNumber = obj.FileArticleName;

		if (renderer.EnableHtmlForInline)
		{
			// write a full a tag
			renderer.Write("<img src=>");

		}
		else
		{
			// inline HTML is disabled, so write the raw value
			renderer.Write('#').Write(obj.FileArticleName);
		}
	}
}
