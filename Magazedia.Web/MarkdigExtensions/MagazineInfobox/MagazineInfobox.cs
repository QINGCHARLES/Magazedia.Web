using Markdig.Parsers;
using Markdig.Syntax;

namespace WikiWikiWorld.MarkdigExtensions;

public class MagazineInfobox : LeafBlock
{
	public Dictionary<string, string> Attributes { get; }

	public MagazineInfobox(BlockParser Parser, Dictionary<string, string> Attributes) : base(Parser)
	{
		this.Attributes = Attributes;
	}
}