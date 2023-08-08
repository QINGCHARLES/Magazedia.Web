using Markdig.Parsers;
using Markdig.Renderers;
using Markdig;

namespace WikiWikiWorld.MarkdigExtensions;

public class MagazineInfoboxExtension : IMarkdownExtension
{
    public void Setup(MarkdownPipelineBuilder Pipeline)
    {
        Pipeline.BlockParsers.InsertBefore<ParagraphBlockParser>(new MagazineInfoboxParser());
    }

    public void Setup(MarkdownPipeline Pipeline, IMarkdownRenderer Renderer)
    {
        if (Renderer is HtmlRenderer HtmlRenderer)
        {
            HtmlRenderer.ObjectRenderers.Add(new MagazineInfoboxHtmlRenderer());
        }
    }
}
