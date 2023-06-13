using Markdig.Renderers;
using Markdig.Renderers.Html;

namespace WikiWikiWorld.MarkdigExtensions;

public class CitationsRenderer : HtmlObjectRenderer<Citations>
{
	private List<Models.Citation> Citations;

	public CitationsRenderer(List<Models.Citation> Citations)
	{
		this.Citations = Citations;
	}

	protected override void Write(HtmlRenderer renderer, Citations obj)
	{
		if (Citations != null && Citations.Count > 0)
		{
			renderer.Write("<ol type=\"1\" class=\"citations\">");

			foreach (Models.Citation Citation in Citations)
			{
				renderer.Write($"<li>{Citation.Text}</li>");
			}

			renderer.Write("</ul>");
		}
	}
}
