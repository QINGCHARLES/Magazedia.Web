using Markdig.Renderers.Html;
using Markdig.Renderers;

namespace WikiWikiWorld.MarkdigExtensions;

public class MagazineInfoboxHtmlRenderer : HtmlObjectRenderer<MagazineInfobox>
{
    protected override void Write(HtmlRenderer Renderer, MagazineInfobox Obj)
    {
        Renderer.Write("<aside class=\"infobox\"><ul>");

        foreach (KeyValuePair<string, string> pair in Obj.Attributes)
        {
            Renderer.Write("<li>").Write(pair.Key).Write(": ").Write(pair.Value).Write("</li>");
        }
        
        Renderer.Write("</ul></aside>");
    }
}