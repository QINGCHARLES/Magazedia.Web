using Markdig;
using Markdig.Helpers;
using Markdig.Parsers;
using Markdig.Renderers;

namespace WikiWikiWorld.MarkdigExtensions;

public class CategoriesExtension : IMarkdownExtension
{
	private readonly List<WikiWikiWorld.Models.Category> Categories;

	public CategoriesExtension(List<WikiWikiWorld.Models.Category> Categories)
	{
		this.Categories = Categories;
	}

	public void Setup(MarkdownPipelineBuilder pipeline)
	{
		OrderedList<InlineParser> parsers;

		parsers = pipeline.InlineParsers;

		if (!parsers.Contains<CategoriesParser>())
		{
			parsers.Add(new CategoriesParser());
		}
	}

	public void Setup(MarkdownPipeline pipeline, IMarkdownRenderer renderer)
	{
		HtmlRenderer? htmlRenderer;
		ObjectRendererCollection? renderers;

		htmlRenderer = renderer as HtmlRenderer;
		renderers = htmlRenderer?.ObjectRenderers;

		if (renderers != null && !renderers.Contains<CategoriesRenderer>())
		{
			renderers!.Add(new CategoriesRenderer(Categories));
		}
	}
}
