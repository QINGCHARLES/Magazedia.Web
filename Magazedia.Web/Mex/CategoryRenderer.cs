using Markdig.Renderers;
using Markdig.Renderers.Html;

namespace WikiWikiWorld.MarkdigExtensions;

public class CategoryRenderer : HtmlObjectRenderer<Category>
{
	private List<WikiWikiWorld.Models.Category> Categories;

	public CategoryRenderer(List<WikiWikiWorld.Models.Category> Categories)
	{
		this.Categories = Categories;
	}

	protected override void Write(HtmlRenderer renderer, Category obj)
	{
		// TODO: Handle {{Category}} tags with custom UrlSlugs
		Categories.Add(new WikiWikiWorld.Models.Category(obj.Data.ToString()));
	}
}
