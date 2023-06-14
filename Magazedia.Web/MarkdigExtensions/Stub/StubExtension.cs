using Markdig;
using Markdig.Helpers;
using Markdig.Parsers;
using Markdig.Renderers;

namespace WikiWikiWorld.MarkdigExtensions;

public class StubExtension : IMarkdownExtension
{
	private readonly List<WikiWikiWorld.Models.Category> Categories;

	public StubExtension(List<WikiWikiWorld.Models.Category> Categories)
	{
		this.Categories = Categories;
	}

	public void Setup(MarkdownPipelineBuilder pipeline)
	{
		OrderedList<InlineParser> parsers;

		parsers = pipeline.InlineParsers;

		if (!parsers.Contains<StubParser>())
		{
			parsers.Add(new StubParser());
		}
	}

	public void Setup(MarkdownPipeline pipeline, IMarkdownRenderer renderer)
	{
		HtmlRenderer? htmlRenderer;
		ObjectRendererCollection? renderers;

		htmlRenderer = renderer as HtmlRenderer;
		renderers = htmlRenderer?.ObjectRenderers;

		if (renderers != null && !renderers.Contains<StubRenderer>())
		{
			renderers!.Add(new StubRenderer(Categories));
		}
	}
}
