using Markdig.Parsers;
using System.Diagnostics;

namespace WikiWikiWorld.MarkdigExtensions;

public class DownloadsBoxParser : BlockParser
{
    public DownloadsBoxParser()
    {
        OpeningCharacters = new[] { '{' };
    }

    public override BlockState TryOpen(BlockProcessor Processor)
    {
		// Check if current line starts with "{{DownloadsBox"
		if (!Processor.Line.Match("{{DownloadsBox "))
		{
			return BlockState.None;
		}

		// Extract data
		int DirectiveStart = Processor.Line.Start;
		int DataStart = DirectiveStart + "{{DownloadsBox ".Length;
		int DataEnd = Processor.Line.IndexOf("}}");
		string DataString = Processor.Line.Text.Substring(DataStart, DataEnd - DataStart);

		string[] DownloadsString = DataString.Split("|#|", StringSplitOptions.TrimEntries); ;
		List<Dictionary<string, string>> Downloads = new();

		foreach (string DownloadString in DownloadsString)
		{
			// Parse data into pairs
			string[] Pairs = DownloadString.Split('|', StringSplitOptions.TrimEntries);
			Dictionary<string, string> Data = Pairs.Select(pair =>
			{
				string[] parts = pair.Split('=', 2);
				return new { Var = parts[0], Text = parts[1] };
			}).ToDictionary(x => x.Var, x => x.Text);

			Downloads.Add(Data);
		}

		DownloadsBox DownloadsBox = new(this, Downloads);
		Processor.NewBlocks.Push(DownloadsBox);

		return BlockState.ContinueDiscard;
	}
}
