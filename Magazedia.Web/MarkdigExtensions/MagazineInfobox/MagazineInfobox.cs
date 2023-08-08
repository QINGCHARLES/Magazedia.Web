using Markdig.Parsers;
using Markdig.Syntax;

namespace WikiWikiWorld.MarkdigExtensions;

public class MagazineInfobox : LeafBlock
{
	public MagazineInfobox(BlockParser Parser, Dictionary<string, string> Data) : base(Parser)
	{
		this.Data = Data;
	}

	public Dictionary<string, string> Data { get; }
}