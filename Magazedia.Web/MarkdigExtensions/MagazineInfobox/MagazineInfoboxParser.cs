using Markdig.Parsers;

namespace WikiWikiWorld.MarkdigExtensions;

public class MagazineInfoboxParser : BlockParser
{
    public MagazineInfoboxParser()
    {
        OpeningCharacters = new[] { '{' };
    }

    public override BlockState TryOpen(BlockProcessor Processor)
    {
        // Check if current line starts with "{{MagazineInfobox"
        if (!Processor.Line.Match("{{MagazineInfobox "))
        {
            return BlockState.None;
        }

        // Extract data
        int DirectiveStart = Processor.Line.Start;
        int DataStart = DirectiveStart + "{{MagazineInfobox ".Length;
        int DataEnd = Processor.Line.IndexOf("}}");
        string DataString = Processor.Line.Text.Substring(DataStart, DataEnd - DataStart);

        // Parse data into pairs
        string[] Pairs = DataString.Split('|',StringSplitOptions.TrimEntries);
        Dictionary<string, string> Data = Pairs.Select(pair =>
        {
            string[] parts = pair.Split('=', 2);
            return new { Var = parts[0], Text = parts[1] };
        }).ToDictionary(x => x.Var, x => x.Text);

		MagazineInfobox MagazineInfobox = new MagazineInfobox(this, Data);
        Processor.NewBlocks.Push(MagazineInfobox);

        return BlockState.ContinueDiscard;
    }
}
