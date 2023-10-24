using Markdig.Parsers;
using Markdig.Renderers;
using Markdig;
using Microsoft.Data.SqlClient;

namespace WikiWikiWorld.MarkdigExtensions;

public class MagazineInfoboxExtension : IMarkdownExtension
{
	private readonly int SiteId;
	private readonly SqlConnection Connection;

	public MagazineInfoboxExtension(int SiteId, SqlConnection Connection)
	{
		this.SiteId = SiteId;
		this.Connection = Connection;
	}

	public void Setup(MarkdownPipelineBuilder Pipeline)
    {
        Pipeline.BlockParsers.InsertBefore<ParagraphBlockParser>(new MagazineInfoboxParser());
    }

    public void Setup(MarkdownPipeline Pipeline, IMarkdownRenderer Renderer)
    {
        if (Renderer is HtmlRenderer HtmlRenderer)
        {
            HtmlRenderer.ObjectRenderers.Add(new MagazineInfoboxHtmlRenderer(SiteId, Connection));
        }
    }
}
