using Markdig.Helpers;
using Markdig.Syntax.Inlines;

namespace WikiWikiWorld.MarkdigExtensions;
public class ShortDescription : LeafInline
{
	public StringSlice Description { get; set; }
}