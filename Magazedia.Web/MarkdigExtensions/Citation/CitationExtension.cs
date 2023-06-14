using Markdig;
using Markdig.Helpers;
using Markdig.Parsers;
using Markdig.Renderers;

namespace WikiWikiWorld.MarkdigExtensions;

public class CitationExtension : IMarkdownExtension
{
	private readonly List<WikiWikiWorld.Models.Citation> Citations;

	public CitationExtension(List<WikiWikiWorld.Models.Citation> Citations)
	{
		this.Citations = Citations;
	}

	public void Setup(MarkdownPipelineBuilder pipeline)
	{
		OrderedList<InlineParser> parsers;

		parsers = pipeline.InlineParsers;

		if (!parsers.Contains<CitationParser>())
		{
			parsers.Add(new CitationParser());
		}
	}

	public void Setup(MarkdownPipeline pipeline, IMarkdownRenderer renderer)
	{
		HtmlRenderer? htmlRenderer;
		ObjectRendererCollection? renderers;

		htmlRenderer = renderer as HtmlRenderer;
		renderers = htmlRenderer?.ObjectRenderers;

		if (renderers != null && !renderers.Contains<CitationRenderer>())
		{
			renderers!.Add(new CitationRenderer(Citations));
		}
	}
}
