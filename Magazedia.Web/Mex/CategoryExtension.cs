using Markdig;
using Markdig.Helpers;
using Markdig.Parsers;
using Markdig.Renderers;

namespace WikiWikiWorld.MarkdigExtensions;

public class CategoryExtension : IMarkdownExtension
{
	private readonly List<WikiWikiWorld.Models.Category> Categories;

	public CategoryExtension(List<WikiWikiWorld.Models.Category> Categories)
	{
		this.Categories = Categories;
	}

	public void Setup(MarkdownPipelineBuilder pipeline)
	{
		OrderedList<InlineParser> parsers;

		parsers = pipeline.InlineParsers;

		if (!parsers.Contains<CategoryParser>())
		{
			parsers.Add(new CategoryParser());
		}
	}

	public void Setup(MarkdownPipeline pipeline, IMarkdownRenderer renderer)
	{
		HtmlRenderer? htmlRenderer;
		ObjectRendererCollection? renderers;

		htmlRenderer = renderer as HtmlRenderer;
		renderers = htmlRenderer?.ObjectRenderers;

		if (renderers != null && !renderers.Contains<CategoryRenderer>())
		{
			renderers!.Add(new CategoryRenderer(Categories));
		}
	}
}
