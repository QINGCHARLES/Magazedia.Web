using Markdig.Parsers;
using Markdig.Renderers;
using Markdig;

namespace WikiWikiWorld.MarkdigExtensions;

public class DownloadBoxExtension : IMarkdownExtension
{
	private readonly List<Models.Download> Downloads;

	public DownloadBoxExtension(List<Models.Download> Downloads)
	{
		this.Downloads = Downloads;
	}

	public void Setup(MarkdownPipelineBuilder Pipeline)
    {
        Pipeline.BlockParsers.InsertBefore<ParagraphBlockParser>(new DownloadBoxParser());
    }

    public void Setup(MarkdownPipeline Pipeline, IMarkdownRenderer Renderer)
    {
        if (Renderer is HtmlRenderer HtmlRenderer)
        {
            HtmlRenderer.ObjectRenderers.Add(new DownloadBoxRenderer(Downloads));
        }
    }
}
