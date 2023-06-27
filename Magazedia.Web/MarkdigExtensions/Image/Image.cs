using Markdig.Helpers;
using Markdig.Syntax.Inlines;

namespace WikiWikiWorld.MarkdigExtensions;

// {{Image XXX}}

public class Image : LeafInline
{
	public StringSlice Data { get; set; }
}
