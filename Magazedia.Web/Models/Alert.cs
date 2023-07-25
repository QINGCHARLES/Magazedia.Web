namespace WikiWikiWorld.Models;

public class Alert
{
	public enum AlertIcons
	{
		Warning,
		Information,
		Crying,
		Disappointed
	}

	public AlertIcons Icon { get; set; }
	public string Markup { get; set; }

	public Alert(AlertIcons Icon, string Markup)
	{
		this.Icon = Icon;
		this.Markup = Markup;
	}
}
