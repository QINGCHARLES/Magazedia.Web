using Markdig.Renderers.Html;
using Markdig.Renderers;
using Magazedia;
using Microsoft.Data.SqlClient;
using Microsoft.AspNetCore.Components.RenderTree;

namespace WikiWikiWorld.MarkdigExtensions;

public class MagazineInfoboxHtmlRenderer : HtmlObjectRenderer<MagazineInfobox>
{
	private int SiteId;
	private SqlConnection Connection;

	public MagazineInfoboxHtmlRenderer(int SiteId, SqlConnection Connection)
	{
		this.SiteId = SiteId;
		this.Connection = Connection;
	}

	protected override void Write(HtmlRenderer Renderer, MagazineInfobox Obj)
    {
        Renderer.Write("<aside class=\"infobox\">");
		
        string? PrimaryCoverImageUrlSlug;
		string? PrimaryCoverImageCaption;

		if (Obj.Attributes.TryGetValue("PrimaryCoverImageUrlSlug", out PrimaryCoverImageUrlSlug) && !string.IsNullOrWhiteSpace(PrimaryCoverImageUrlSlug))
		{
			PrimaryCoverImageUrlSlug = PrimaryCoverImageUrlSlug.Trim();
			(string FileName, string Title) = Helpers.GetImageFilenameAndArticleTitleFromArticleUrlSlug(PrimaryCoverImageUrlSlug, Connection);
			// TODO: This alt text should probably take into account the caption given under the image
			// as well as the title of the actual image article in the db
			Renderer.Write($"<img src=\"/sitefiles/1/images/{FileName}\" alt=\"{Title}\" />");
			Obj.Attributes.Remove("PrimaryCoverImageUrlSlug");
		}

		Renderer.Write("<ul>");
		foreach (KeyValuePair<string, string> pair in Obj.Attributes)
        {
            Renderer.Write("<li><strong>").Write(pair.Key).Write(":</strong> ").Write(pair.Value).Write("</li>");
        }
        
        Renderer.Write("</ul></aside>");
    }
}