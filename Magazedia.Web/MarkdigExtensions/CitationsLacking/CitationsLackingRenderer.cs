using Markdig.Renderers;
using Markdig.Renderers.Html;

namespace WikiWikiWorld.MarkdigExtensions;

public class CitationsLackingRenderer : HtmlObjectRenderer<CitationsLacking>
{
	private List<Models.Category> Categories;

	public CitationsLackingRenderer(List<Models.Category> Categories)
	{
		this.Categories = Categories;
	}

	protected override void Write(HtmlRenderer renderer, CitationsLacking obj)
	{
		renderer.Write("<p>This article is <a href=\"/wiki:citations\">lacking any or sufficient citations</a>. Help us by adding some.</p>");
		if (Categories != null)
		{
			Categories.Add(new Models.Category("All articles lacking citations", Models.Category.PriorityOptions.Secondary));
			if (obj.Data.Length > 0)
			{
				Categories.Add(new Models.Category(obj.Data.ToString(), Models.Category.PriorityOptions.Secondary));
			}
		}
	}
}
