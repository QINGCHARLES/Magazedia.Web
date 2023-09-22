using Markdig.Parsers;
using System.Diagnostics;

namespace WikiWikiWorld.MarkdigExtensions;

public class DownloadBoxParser : BlockParser
{
    public DownloadBoxParser()
    {
        OpeningCharacters = new[] { '{' };
    }

    public override BlockState TryOpen(BlockProcessor Processor)
    {
		// Check if current line starts with "{{DownloadBox}}"
		if (Processor.Line.Match("{{DownloadBox}}"))
		{
			DownloadBox downloadBox = new DownloadBox(this);
			Processor.NewBlocks.Push(downloadBox);

			return BlockState.ContinueDiscard;
		}

		return BlockState.None;
	}
}
