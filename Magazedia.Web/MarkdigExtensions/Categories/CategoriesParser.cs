using Markdig.Parsers;

namespace WikiWikiWorld.MarkdigExtensions;

public class CategoriesParser : BlockParser
{
	public CategoriesParser()
	{
		OpeningCharacters = new[] { '{' };
	}

	public override BlockState TryOpen(BlockProcessor processor)
	{
		if (processor.Line.Match("{{Categories}}"))
		{
			processor.NewBlocks.Push(new Categories(this));
			return BlockState.BreakDiscard;
		}

		return BlockState.None;
	}
}

