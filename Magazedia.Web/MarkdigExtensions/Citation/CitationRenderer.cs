using Markdig.Renderers;
using Markdig.Renderers.Html;

namespace WikiWikiWorld.MarkdigExtensions;

public class CitationRenderer : HtmlObjectRenderer<Citation>
{
	private List<Models.Citation> Citations;

	public CitationRenderer(List<Models.Citation> Citations)
	{
		this.Citations = Citations;
	}

	protected override void Write(HtmlRenderer renderer, Citation obj)
	{
		Citations.Add(new Models.Citation(obj.Data.ToString()));
		renderer.Write($"<sup><a href=\"#citation{Citations.Count}\">{Citations.Count}</a></sup>");
	}
}
