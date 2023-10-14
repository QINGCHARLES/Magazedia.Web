using Markdig.Renderers.Html;
using Markdig.Renderers;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;
using Magazedia;

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
		Renderer.Write("<aside class=\"infobox\"><h1>Downloads</h1>");

		foreach (Dictionary<string, string> Download in Obj.Downloads)
		{
			string HashHexString = Download["Hash"];
			byte[] Hash = HexStringToByteArray(HashHexString);

			//Renderer.Write("<li><a href=\"").Write(Download["PrimaryUrl"]).Write("\" rel=\"nofollow\">Link</a></li>");

			string Sql = "SELECT * FROM DownloadUrls WHERE SiteId = @SiteId AND HashSha256 = @Hash";
			WikiWikiWorld.Models.DownloadUrl DownloadUrl = Connection.QueryFirstOrDefault<WikiWikiWorld.Models.DownloadUrl>(Sql, new { SiteId, Hash });

			if (DownloadUrl != null)
			{
				//Renderer.Write(DownloadUrl.Filename + "<br>");

				string[] Urls = DownloadUrl.DownloadUrls.Split("|");
				string LinkText = "Primary Download";

				foreach (string Url in Urls)
				{
					//Renderer.Write("<li><a href=\"").Write(Url).Write("\" rel=\"nofollow\">" + LinkText + "</a></li>");
					string g = $@"
<div class=""download-container"">
<div class=""dicon""></div>
<div class=""dinfo"">
    <div><a href=""{Url}"">{DownloadUrl.Filename}</a></div>
    <div class=""download-description"">{Download["Description"]}</div>
    <div class=""download-filesize"">
	{Helpers.HumanReadableByteCount(DownloadUrl.FileSizeBytes, true)}
    </div>
  </div>
</div>
					
					";

					Renderer.Write(g);

					LinkText = "Alternate Download";
				}

				Renderer.Write("");
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