using Markdig.Renderers;
using Markdig.Renderers.Html;

namespace WikiWikiWorld.MarkdigExtensions;

public class StubRenderer : HtmlObjectRenderer<Stub>
{
	private List<Models.Category> Categories;

	public StubRenderer(List<Models.Category> Categories)
	{
		this.Categories = Categories;
	}

	protected override void Write(HtmlRenderer renderer, Stub obj)
	{
		renderer.Write("<p>This article is a <a href=\"/help:stub-articles\">stub</a>. Help us by expanding it.</p>");
		if (Categories != null)
		{
			Categories.Add(new Models.Category("All stub articles", Models.Category.PriorityOptions.Secondary));
			
			// If the tag isn't empty then use the supplied Data to add an additional custom Category tag to the page
			if (obj.Data.Length > 0)
			{
				Categories.Add(new Models.Category(obj.Data.ToString(), Models.Category.PriorityOptions.Secondary));
			}
		}
	}
}
