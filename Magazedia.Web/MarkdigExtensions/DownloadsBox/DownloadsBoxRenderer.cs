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
		Renderer.Write("<aside class=\"infobox\">");

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
					string g = $@"<section class=""rewards"" aria-label=""Rewards"">
  <div class=""download"">
    <p>
      <a download href=""movie.mov"">Download movie</a>
    </p>
    <p class=""download__instructions"">
      <small>Typically takes 30&ndash;45 minutes.</small>
      <small class=""download__properties"">2.4 <abbr title=""Gigabytes"">GB</abbr> AVI</small>
    </p>
  </div>

  <div class=""download"">
    <p>
      <a download href=""theme-song.mp3"">Download theme song</a>
    </p>
    <p class=""download__instructions"">
      <small>Typically takes 15&ndash;30 seconds.</small>
      <small class=""download__properties"">4.5 <abbr title=""Megabytes"">MB</abbr> MP3</small>
    </p>
  </div></section>";


					string f = @$"	<a href=""{Url}"" rel=""nofollow"" download>
										<button class=""download-button"">
											<div class=""icon-container"">
												<svg class=""download-icon"">
<path fill-rule=""evenodd"" clip-rule=""evenodd"" d=""M13.3099 18.6881C12.5581 19.3396 11.4419 19.3396 10.6901 18.6881L5.87088 14.5114C4.47179 13.2988 5.32933 11 7.18074 11L9.00001 11V3C9.00001 1.89543 9.89544 1 11 1L13 1C14.1046 1 15 1.89543 15 3L15 11H16.8193C18.6707 11 19.5282 13.2988 18.1291 14.5114L13.3099 18.6881ZM11.3451 16.6091C11.7209 16.9348 12.2791 16.9348 12.6549 16.6091L16.8193 13H14.5C13.6716 13 13 12.3284 13 11.5V3L11 3V11.5C11 12.3284 10.3284 13 9.50001 13L7.18074 13L11.3451 16.6091Z"" fill=""#0F0F0F""/></svg>
												<path fill-rule=""evenodd"" clip-rule=""evenodd"" d=""M23 22C23 22.5523 22.5523 23 22 23H2C1.44772 23 1 22.5523 1 22C1 21.4477 1.44772 21 2 21H22C22.5523 21 23 21.4477 23 22Z"" fill=""#0F0F0F""/>

											</div>
											<div class=""text-container"">
												<span class=""download-text"">Download</span>
												<span class=""file-info"">
													<span class=""filename"">{DownloadUrl.Filename}</span>
													<span class=""filesize"">2MB</span>
												</span>
											</div>
											<div class=""filetype-icon-container"">
												<svg class=""filetype-icon""></svg>
											</div>
											<div class=""primary-indicator"">
												{LinkText}
											</div>
										</button>
									</a>";
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