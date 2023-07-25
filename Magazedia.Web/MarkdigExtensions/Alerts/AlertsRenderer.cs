using Markdig.Renderers;
using Markdig.Renderers.Html;

namespace WikiWikiWorld.MarkdigExtensions;

public class AlertsRenderer : HtmlObjectRenderer<Alerts>
{
	private List<Models.Alert> Alerts;

	public AlertsRenderer(List<Models.Alert> Alerts)
	{
		this.Alerts = Alerts;
	}

	protected override void Write(HtmlRenderer renderer, Alerts obj)
	{
		foreach (var Alert in Alerts)
		{
			renderer.Write("<aside roll=\"note\">[icon:" + Alert.Icon.ToString() + "] " + Alert.Markup + "</aside>");
		}
	}
}
