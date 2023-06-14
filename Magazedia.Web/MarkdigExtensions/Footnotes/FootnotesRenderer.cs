using Markdig.Renderers;
using Markdig.Renderers.Html;

namespace WikiWikiWorld.MarkdigExtensions;

public class FootnotesRenderer : HtmlObjectRenderer<Footnotes>
{
	private List<Models.Footnote> Footnotes;

	public FootnotesRenderer(List<Models.Footnote> Footnotes)
	{
		this.Footnotes = Footnotes;
	}

	protected override void Write(HtmlRenderer renderer, Footnotes obj)
	{
		if (Footnotes != null && Footnotes.Count > 0)
		{
			renderer.Write("<ol type=\"a\" class=\"footnotes\">");

			int FootnoteNumber = 1;
			foreach (Models.Footnote Footnote in Footnotes)
			{
				renderer.Write($"<li><a name=\"footnote{Magazedia.Helpers.ConvertNumberToLetters(FootnoteNumber++)}\"></a>{Footnote.Text}</li>");
			}

			renderer.Write("</ol>");
		}
	}
}
