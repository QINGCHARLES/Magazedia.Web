using Markdig;
using Markdig.Helpers;
using Markdig.Parsers;
using Markdig.Renderers;

namespace WikiWikiWorld.MarkdigExtensions;

public class CitationsExtension : IMarkdownExtension
{
	private readonly List<WikiWikiWorld.Models.Citation> Citations;

	public CitationsExtension(List<WikiWikiWorld.Models.Citation> Citations)
	{
		this.Citations = Citations;
	}

	public void Setup(MarkdownPipelineBuilder pipeline)
	{
		OrderedList<InlineParser> parsers;

		parsers = pipeline.InlineParsers;

		if (!parsers.Contains<CitationsParser>())
		{
			parsers.Add(new CitationsParser());
		}
	}

	public void Setup(MarkdownPipeline pipeline, IMarkdownRenderer renderer)
	{
		HtmlRenderer? htmlRenderer;
		ObjectRendererCollection? renderers;

		htmlRenderer = renderer as HtmlRenderer;
		renderers = htmlRenderer?.ObjectRenderers;

		if (renderers != null && !renderers.Contains<CitationsRenderer>())
		{
			renderers!.Add(new CitationsRenderer(Citations));
		}
	}
}
