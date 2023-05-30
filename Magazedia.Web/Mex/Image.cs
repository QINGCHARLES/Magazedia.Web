using Markdig.Helpers;
using Markdig.Syntax.Inlines;

namespace WikiWikiWorld.MarkdigExtensions;
public class Image : LeafInline
{
	public StringSlice FileArticleName { get; set; }
	public StringSlice AltText { get; set; }

}
