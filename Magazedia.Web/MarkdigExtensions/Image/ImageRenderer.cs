using Markdig.Helpers;
using Markdig.Renderers;
using Markdig.Renderers.Html;
using Microsoft.Data.SqlClient;

namespace WikiWikiWorld.MarkdigExtensions;

public class ImageRenderer : HtmlObjectRenderer<Image>
{
	private int SiteId;
	private SqlConnection SqlConnection;

	public ImageRenderer(int SiteId, SqlConnection SqlConnection)
	{
		this.SiteId = SiteId;
		this.SqlConnection = SqlConnection;
	}

	protected override void Write(HtmlRenderer renderer, Image obj)
	{
		StringSlice Data;

		Data = obj.Data;

		string[] DataParts = obj.Data.ToString().Split("|");
		Dictionary<string,string> ImageAttributes = new();

		ImageAttributes.Add("Filename", DataParts[0]);

		for(int DataPart = 1; DataPart < DataParts.Length; DataPart++)
		{
			string AttributeName = DataParts[DataPart].Split("=")[0];
			string AttributeValue = DataParts[DataPart].Split("=")[1];
			ImageAttributes.Add(AttributeName, AttributeValue);
		}

		if (renderer.EnableHtmlForInline)
		{
			string SqlQuery = $"";
            // SqlConnection
            // write a full a tag
            renderer.Write($"<img style=\"width: 15rem; float:left;\" src=\"/sitefiles/{SiteId}/{ImageAttributes["Filename"].Split("file:", 2)[1]}\">");

		}
		else
		{
			// inline HTML is disabled, so write the raw value
			renderer.Write('#').Write(obj.Data);
		}
	}
}
