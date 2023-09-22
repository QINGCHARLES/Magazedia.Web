using Markdig.Renderers.Html;
using Markdig.Renderers;

namespace WikiWikiWorld.MarkdigExtensions;

public class DownloadBoxRenderer : HtmlObjectRenderer<DownloadBox>
{
	private List<Models.Download> Downloads;

	public DownloadBoxRenderer(List<Models.Download> Downloads)
	{
		this.Downloads = Downloads;
	}

	protected override void Write(HtmlRenderer Renderer, DownloadBox Obj)
    {
        Renderer.Write("<aside class=\"infobox\"><ul>");

        foreach(var Download in Downloads) 
		{
			    Renderer.Write("<li>").Write(Download.Filename).Write("</li>");
		}

		Renderer.Write("</ul></aside>");
    }
}