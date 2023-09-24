using Markdig.Renderers.Html;
using Markdig.Renderers;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;

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
		Renderer.Write("<aside class=\"infobox\">Downloads available:<br><ul>");

		foreach (Dictionary<string, string> Download in Obj.Downloads)
		{
			string HashHexString = Download["Hash"];
			byte[] Hash = HexStringToByteArray(HashHexString);

			//Renderer.Write("<li><a href=\"").Write(Download["PrimaryUrl"]).Write("\" rel=\"nofollow\">Link</a></li>");

			string Sql = "SELECT * FROM DownloadUrls WHERE SiteId = @SiteId AND HashSha256 = @Hash";
			WikiWikiWorld.Models.DownloadUrl DownloadUrl = Connection.QueryFirstOrDefault<WikiWikiWorld.Models.DownloadUrl>(Sql, new { SiteId, Hash });

			if (DownloadUrl != null)
			{
				Renderer.Write(DownloadUrl.Filename + "<br><ul>");

				string[] Urls = DownloadUrl.DownloadUrls.Split("|");
				string LinkText = "Primary Download";

				foreach (string Url in Urls)
				{
					Renderer.Write("<li><a href=\"").Write(Url).Write("\" rel=\"nofollow\">" + LinkText + "</a></li>");
					LinkText = "Alternate Download";
				}

				Renderer.Write("</ul>");
			}
		}

		Renderer.Write("</aside>");
	}

	public static byte[] HexStringToByteArray(string Hex)
	{
		return Enumerable.Range(0, Hex.Length)
						 .Where(x => x % 2 == 0)
						 .Select(x => Convert.ToByte(Hex.Substring(x, 2), 16))
						 .ToArray();
	}
}