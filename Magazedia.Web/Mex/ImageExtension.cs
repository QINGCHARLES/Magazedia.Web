using Markdig;
using Markdig.Helpers;
using Markdig.Parsers;
using Markdig.Renderers;
using MarkdigMantisLink;

namespace WikiWikiWorld.MarkdigExtensions;

public class ImageExtension : IMarkdownExtension
{
	public void Setup(MarkdownPipelineBuilder pipeline)
	{
		OrderedList<InlineParser> parsers;

		parsers = pipeline.InlineParsers;

		if (!parsers.Contains<ImageParser>())
		{
			parsers.Add(new ImageParser());
		}
	}

	public void Setup(MarkdownPipeline pipeline, IMarkdownRenderer renderer)
	{
		HtmlRenderer? htmlRenderer;
		ObjectRendererCollection? renderers;

		htmlRenderer = renderer as HtmlRenderer;
		renderers = htmlRenderer?.ObjectRenderers;

		if (renderers != null && !renderers.Contains<ImageRenderer>())
		{
			renderers!.Add(new ImageRenderer());
		}
	}
}
