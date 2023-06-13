using Markdig.Helpers;
using Markdig.Syntax.Inlines;

namespace WikiWikiWorld.MarkdigExtensions;
public class Stub : LeafInline 
{
	public StringSlice Data { get; set; }
}
