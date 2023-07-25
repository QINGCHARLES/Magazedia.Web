using Markdig;
using Markdig.Helpers;
using Markdig.Parsers;
using Markdig.Renderers;

namespace WikiWikiWorld.MarkdigExtensions;

public class AlertsExtension : IMarkdownExtension
{
	private readonly List<WikiWikiWorld.Models.Alert> Alerts;

	public AlertsExtension(List<WikiWikiWorld.Models.Alert> Alerts)
	{
		this.Alerts = Alerts;
	}

	public void Setup(MarkdownPipelineBuilder pipeline)
	{
		OrderedList<BlockParser> parsers;

		parsers = pipeline.BlockParsers;

		if (!parsers.Contains<AlertsParser>())
		{
			parsers.Add(new AlertsParser());
		}
	}

	public void Setup(MarkdownPipeline pipeline, IMarkdownRenderer renderer)
	{
		HtmlRenderer? htmlRenderer;
		ObjectRendererCollection? renderers;

		htmlRenderer = renderer as HtmlRenderer;
		renderers = htmlRenderer?.ObjectRenderers;

		if (renderers != null && !renderers.Contains<AlertsRenderer>())
		{
			renderers!.Add(new AlertsRenderer(Alerts));
		}
	}
}
