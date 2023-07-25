using Markdig.Parsers;
using Markdig.Syntax;

namespace WikiWikiWorld.MarkdigExtensions;

public class Alerts : LeafBlock
{
	public Alerts(BlockParser Parser) : base(Parser) { }
}
