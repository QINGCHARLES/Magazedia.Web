using Markdig.Helpers;
using Markdig.Syntax.Inlines;

namespace WikiWikiWorld.MarkdigExtensions;
public class Citation : LeafInline
{
	public StringSlice Data { get; set; }
}
