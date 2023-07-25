using Markdig.Renderers;
using Markdig.Renderers.Html;

namespace WikiWikiWorld.MarkdigExtensions;

public class StubRenderer : HtmlObjectRenderer<Stub>
{
	private List<Models.Alert> Alert;
	private List<Models.Category> Categories;

	public StubRenderer(List<Models.Alert> Alert, List<Models.Category> Categories)
	{
		this.Alert = Alert;
		this.Categories = Categories;
	}

	protected override void Write(HtmlRenderer renderer, Stub obj)
	{
		int PipeIndex = obj.Data.IndexOf("Custom|");

		if (PipeIndex != -1)
		{
			// TODO: Custom stubs
			//turn obj.Data.Substring(PipeIndex + 1);
		}
		else
		{
			switch(obj.Data.ToString().ToUpper())
			{
				// Generic default Stub type
				default:
				case "":
					Alert.Add(new Models.Alert(Models.Alert.AlertIcons.Crying, "This article is a stub."));
					Categories.Add(new Models.Category("All stub articles", Models.Category.PriorityOptions.Secondary));
					break;
				case "MAGAZINE ISSUE":
					Alert.Add(new Models.Alert(Models.Alert.AlertIcons.Crying, "This magazine article is a magazine."));
					Categories.Add(new Models.Category("All stub articles", Models.Category.PriorityOptions.Secondary));
					Categories.Add(new Models.Category("Magazine stub articles", Models.Category.PriorityOptions.Secondary));
					break;
			}

			//if (Alert != null)
			//{
			//	Alert.Add(new Models.Alert(Models.Alert.AlertIcons.Crying, "This article is a stub."));
			//}

			//if (Categories != null)
			//{
			//	Categories.Add(new Models.Category("All stub articles", Models.Category.PriorityOptions.Secondary));

			//	// If the tag isn't empty then use the supplied Data to add an additional custom Category tag to the page
			//	if (obj.Data.Length > 0)
			//	{
			//		Categories.Add(new Models.Category(obj.Data.ToString(), Models.Category.PriorityOptions.Secondary));
			//	}
			//}
		}

	}
}
