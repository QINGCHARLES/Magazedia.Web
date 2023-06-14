using Markdig;
using Markdig.Helpers;
using Markdig.Parsers;
using Markdig.Renderers;

namespace WikiWikiWorld.MarkdigExtensions;

public class FootnoteExtension : IMarkdownExtension
{
	private readonly List<Models.Footnote> Footnotes;

	public FootnoteExtension(List<Models.Footnote> Footnotes)
	{
		this.Footnotes = Footnotes;
	}

	public void Setup(MarkdownPipelineBuilder pipeline)
	{
		OrderedList<InlineParser> parsers;

		parsers = pipeline.InlineParsers;

		if (!parsers.Contains<FootnoteParser>())
		{
			parsers.Add(new FootnoteParser());
		}
	}

	public void Setup(MarkdownPipeline pipeline, IMarkdownRenderer renderer)
	{
		HtmlRenderer? htmlRenderer;
		ObjectRendererCollection? renderers;

		htmlRenderer = renderer as HtmlRenderer;
		renderers = htmlRenderer?.ObjectRenderers;

		if (renderers != null && !renderers.Contains<FootnoteRenderer>())
		{
			renderers!.Add(new FootnoteRenderer(Footnotes));
		}
	}
}
