using Markdig;
using Markdig.Helpers;
using Markdig.Parsers;
using Markdig.Renderers;

namespace WikiWikiWorld.MarkdigExtensions;

public class CitationsLackingExtension : IMarkdownExtension
{
	private readonly List<Models.Category> Categories;

	public CitationsLackingExtension(List<Models.Category> Categories)
	{
		this.Categories = Categories;
	}

	public void Setup(MarkdownPipelineBuilder pipeline)
	{
		OrderedList<InlineParser> parsers;

		parsers = pipeline.InlineParsers;

		if (!parsers.Contains<CitationsLackingParser>())
		{
			parsers.Add(new CitationsLackingParser());
		}
	}

	public void Setup(MarkdownPipeline pipeline, IMarkdownRenderer renderer)
	{
		HtmlRenderer? htmlRenderer;
		ObjectRendererCollection? renderers;

		htmlRenderer = renderer as HtmlRenderer;
		renderers = htmlRenderer?.ObjectRenderers;

		if (renderers != null && !renderers.Contains<CitationsLackingRenderer>())
		{
			renderers!.Add(new CitationsLackingRenderer(Categories));
		}
	}
}
