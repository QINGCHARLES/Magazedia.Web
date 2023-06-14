using Markdig.Helpers;
using Markdig.Syntax.Inlines;

namespace WikiWikiWorld.MarkdigExtensions;
public class Category : LeafInline
{
	public StringSlice Data { get; set; }
}
