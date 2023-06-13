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
		// TODO: Handle {{Category}} tags with custom UrlSlugs
		Citations.Add(new Models.Citation(obj.Data.ToString()));
		renderer.Write($"<sup>{Citations.Count}</sup>");
	}
}
