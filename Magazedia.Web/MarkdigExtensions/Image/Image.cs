using Markdig.Helpers;
using Markdig.Parsers;
using Markdig.Syntax.Inlines;

namespace WikiWikiWorld.MarkdigExtensions;

// {{Image XXX}}

public class Image : LeafInline
{
    public string? UrlSlug { get; set; }
    public Dictionary<string, string>? Attributes { get; set; }
}
