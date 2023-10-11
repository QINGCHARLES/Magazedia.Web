using Markdig.Parsers;
using Markdig.Syntax;

namespace WikiWikiWorld.MarkdigExtensions;

public class Categories : LeafBlock
{
	public Categories(BlockParser Parser) : base(Parser) { }
}