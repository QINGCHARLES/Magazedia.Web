using Markdig;
using Markdig.Helpers;
using Markdig.Parsers;
using Markdig.Renderers;

namespace WikiWikiWorld.MarkdigExtensions;

public class FootnotesExtension : IMarkdownExtension
{
	private readonly List<Models.Footnote> Footnotes;

	public FootnotesExtension(List<Models.Footnote> Footnotes)
	{
		this.Footnotes = Footnotes;
	}

	public void Setup(MarkdownPipelineBuilder pipeline)
	{
		OrderedList<InlineParser> parsers;

		parsers = pipeline.InlineParsers;

		if (!parsers.Contains<FootnotesParser>())
		{
			parsers.Add(new FootnotesParser());
		}
	}

	public void Setup(MarkdownPipeline pipeline, IMarkdownRenderer renderer)
	{
		HtmlRenderer? htmlRenderer;
		ObjectRendererCollection? renderers;

		htmlRenderer = renderer as HtmlRenderer;
		renderers = htmlRenderer?.ObjectRenderers;

		if (renderers != null && !renderers.Contains<FootnotesRenderer>())
		{
			renderers!.Add(new FootnotesRenderer(Footnotes));
		}
	}
}
