using Markdig.Renderers;
using Markdig.Renderers.Html;

namespace WikiWikiWorld.MarkdigExtensions;

public class FootnoteRenderer : HtmlObjectRenderer<Footnote>
{
	private List<Models.Footnote> Footnotes;

	public FootnoteRenderer(List<Models.Footnote> Footnotes)
	{
		this.Footnotes = Footnotes;
	}

	protected override void Write(HtmlRenderer renderer, Footnote obj)
	{
		Footnotes.Add(new Models.Footnote(obj.Data.ToString()));
		renderer.Write($"<sup><a href=\"#footnote{Magazedia.Helpers.ConvertNumberToLetters(Footnotes.Count)}\">{Magazedia.Helpers.ConvertNumberToLetters(Footnotes.Count)}</a></sup>");
	}



}
