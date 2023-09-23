using Markdig.Renderers.Html;
using Markdig.Renderers;
using Dapper;
using Microsoft.Data.SqlClient;

namespace WikiWikiWorld.MarkdigExtensions;

public class DownloadsBoxRenderer : HtmlObjectRenderer<DownloadsBox>
{
	private int SiteId;
	private SqlConnection Connection;

	public DownloadsBoxRenderer(int SiteId, SqlConnection Connection)
	{
		this.SiteId = SiteId;
		this.Connection = Connection;
	}

	protected override void Write(HtmlRenderer Renderer, DownloadsBox Obj)
    {
        Renderer.Write("<aside class=\"infobox\">Downloads available:<ul>");

		foreach(Dictionary<string,string> Download in Obj.Downloads) 
		{
//			Renderer.Write("<li>").Write(Download["Description"]).Write("</li>");
			Renderer.Write("<li><a href=\"").Write(Download["PrimaryUrl"]).Write("\" rel=\"nofollow\">Link</a></li>");
		}

		Renderer.Write("</ul></aside>");
    }
}