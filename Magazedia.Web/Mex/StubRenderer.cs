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
		if (Categories != null)
		{
			Categories.Add(new Models.Category("All stub articles", Models.Category.PriorityOptions.Secondary));
			if (obj.Data.Length > 0)
			{
				Categories.Add(new Models.Category(obj.Data.ToString(), Models.Category.PriorityOptions.Secondary));
			}
		}
	}
}
