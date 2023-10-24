using Markdig.Helpers;
using Markdig.Renderers;
using Markdig.Renderers.Html;
using Microsoft.Data.SqlClient;

namespace WikiWikiWorld.MarkdigExtensions;

public class ImageRenderer : HtmlObjectRenderer<Image>
{
    private int SiteId;
    private SqlConnection Connection;
	private Magazedia.Web.Pages.BasePageModel Page;

	public ImageRenderer(int SiteId, SqlConnection Connection, Magazedia.Web.Pages.BasePageModel Page)
	{
		this.SiteId = SiteId;
		this.Connection = Connection;
		this.Page = Page;
	}

	protected override void Write(HtmlRenderer renderer, Image obj)
    {
        //StringSlice Data;

        //Data = obj.Data;

        //string[] DataParts = obj.Data.ToString().Split("|");
        //Dictionary<string,string> ImageAttributes = new();

        //ImageAttributes.Add("Filename", DataParts[0]);

        //for(int DataPart = 1; DataPart < DataParts.Length; DataPart++)
        //{
        //	string AttributeName = DataParts[DataPart].Split("=")[0];
        //	string AttributeValue = DataParts[DataPart].Split("=")[1];
        //	ImageAttributes.Add(AttributeName, AttributeValue);
        //}

        //if (renderer.EnableHtmlForInline)
        //{
        //	string SqlQuery = $"";
        //          // SqlConnection
        //          // write a full a tag
        //          //renderer.Write($"<img style=\"width: 15rem; float:left;\" src=\"/sitefiles/{SiteId}/{ImageAttributes["Filename"].Split("file:", 2)[1]}\">");
        //}
        //else
        //{
        //	// inline HTML is disabled, so write the raw value
        //	renderer.Write('#').Write(obj.Data);
        //}

        (string FileName, string Title) = Magazedia.Helpers.GetImageFilenameAndArticleTitleFromArticleUrlSlug(obj.UrlSlug!, Connection);

        string? Type;

		if (obj.Attributes.TryGetValue("Type", out Type) && !string.IsNullOrWhiteSpace(Type))
		{
            Type = Type.Trim();
		}

        switch(Type)
        {   // TODO: Why does this need background-size in here???
            case "Header":
				renderer.Write($@"  <style>
                                        h1.title
                                        {{
                                            background: linear-gradient(to bottom, #3338, #fff0), url(/sitefiles/1/images/{FileName}) top center / cover no-repeat;
                                        }}
                                    </style>
                                        ");
                break;
			case null:
			default:    
				renderer.Write($"<img src=\"/sitefiles/1/images/{FileName}\" alt=\"{Title}\" />");
				break;
		}

		//if (obj.Attributes != null)
		//{
		//    foreach (var attribute in obj.Attributes)
		//    {
		//        renderer.Write(attribute.Key).Write("=\"").Write(attribute.Value).Write("\" ");
		//    }
		//}

    }
}
