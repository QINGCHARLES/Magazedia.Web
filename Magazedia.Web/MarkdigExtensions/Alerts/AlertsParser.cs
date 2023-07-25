using Markdig.Parsers;

namespace WikiWikiWorld.MarkdigExtensions;

public class AlertsParser : BlockParser
{
	public AlertsParser()
	{
		OpeningCharacters = new[] { '{' };
	}

	public override BlockState TryOpen(BlockProcessor processor)
	{
		if (processor.Line.Match("{{Alerts}}"))
		{
			processor.NewBlocks.Push(new Alerts(this));
			return BlockState.BreakDiscard;
		}

		return BlockState.None;
	}
}
