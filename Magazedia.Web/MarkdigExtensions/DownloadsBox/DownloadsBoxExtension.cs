using Markdig.Parsers;
using Markdig.Renderers;
using Markdig;
using Microsoft.Data.SqlClient;

namespace WikiWikiWorld.MarkdigExtensions;

public class DownloadsBoxExtension : IMarkdownExtension
{
	private readonly int SiteId;
	private readonly SqlConnection Connection;

	public DownloadsBoxExtension(int SiteId, SqlConnection Connection)
	{
		this.SiteId = SiteId;
		this.Connection = Connection;
	}

	public void Setup(MarkdownPipelineBuilder Pipeline)
    {
        Pipeline.BlockParsers.InsertBefore<ParagraphBlockParser>(new DownloadsBoxParser());
    }

    public void Setup(MarkdownPipeline Pipeline, IMarkdownRenderer Renderer)
    {
        if (Renderer is HtmlRenderer HtmlRenderer)
        {
            HtmlRenderer.ObjectRenderers.Add(new DownloadsBoxRenderer(SiteId, Connection));
        }
    }
}
