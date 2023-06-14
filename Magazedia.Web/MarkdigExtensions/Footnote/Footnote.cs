using Markdig.Helpers;
using Markdig.Syntax.Inlines;

namespace WikiWikiWorld.MarkdigExtensions;
public class Footnote : LeafInline
{
	public StringSlice Data { get; set; }
}
