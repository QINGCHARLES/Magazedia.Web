using Markdig.Helpers;
using Markdig.Syntax.Inlines;

namespace WikiWikiWorld.MarkdigExtensions;
public class CitationsLacking : LeafInline 
{
	public StringSlice Data { get; set; }
}
