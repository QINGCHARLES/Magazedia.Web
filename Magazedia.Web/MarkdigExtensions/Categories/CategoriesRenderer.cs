using Markdig.Renderers;
using Markdig.Renderers.Html;

namespace WikiWikiWorld.MarkdigExtensions;

public class CategoriesRenderer : HtmlObjectRenderer<Categories>
{
	private List<Models.Category> Categories;

	public CategoriesRenderer(List<Models.Category> Categories)
	{
		this.Categories = Categories;
	}

	protected override void Write(HtmlRenderer renderer, Categories obj)
	{
		if (Categories != null && Categories.Count > 0)
		{
			renderer.Write("<ul class=\"categories\">");

			WriteCategories(renderer, Models.Category.PriorityOptions.Primary);
			WriteCategories(renderer, Models.Category.PriorityOptions.Secondary);

			renderer.Write("</ul>");
		}
	}

	private void WriteCategories(HtmlRenderer renderer, Models.Category.PriorityOptions Priority)
	{
		foreach (Models.Category Category in Categories)
		{
			if (Category.Priority == Priority)
			{
				string Url = String.IsNullOrWhiteSpace(Category.UrlSlug) ? Magazedia.Helpers.Slugify(Category.Title) : Category.UrlSlug;
				renderer.Write($"<li><a href=\"/category:{Url}\">{Category.Title}</a></li>");
			}
		}
	}
}
