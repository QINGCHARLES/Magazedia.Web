using Markdig;
using Markdig.Helpers;
using Markdig.Parsers;
using Markdig.Renderers;

namespace WikiWikiWorld.MarkdigExtensions;

public class ShortDescriptionExtension : IMarkdownExtension
{
	private readonly Magazedia.Web.Pages.BasePageModel Page;

	public ShortDescriptionExtension(Magazedia.Web.Pages.BasePageModel Page)
	{
		this.Page = Page;
	}

	public void Setup(MarkdownPipelineBuilder pipeline)
	{
		OrderedList<InlineParser> parsers;

		parsers = pipeline.InlineParsers;

		if (!parsers.Contains<ShortDescriptionParser>())
		{
			parsers.Add(new ShortDescriptionParser());
		}
	}

	public void Setup(MarkdownPipeline pipeline, IMarkdownRenderer renderer)
	{
		HtmlRenderer? htmlRenderer;
		ObjectRendererCollection? renderers;

		htmlRenderer = renderer as HtmlRenderer;
		renderers = htmlRenderer?.ObjectRenderers;

		if (renderers != null && !renderers.Contains<ShortDescriptionRenderer>())
		{
			renderers!.Add(new ShortDescriptionRenderer(Page));
		}
	}
}
