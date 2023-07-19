using Markdig.Helpers;
using Markdig.Parsers;
using MarkdigMantisLink;

namespace WikiWikiWorld.MarkdigExtensions;

public class ImageParser : InlineParser
{
	public ImageParser()
	{
		OpeningCharacters = new[] { '{' };
	}

	public override bool Match(InlineProcessor Processor, ref StringSlice Slice)
	{
		if (!Slice.Match("{{Image ")) return false;

		int Start = Slice.Start;
		Slice.Start += 8;

		int End = Slice.IndexOf("}}");
		if (End == -1) return false;

		Processor.Inline = new Image { Data = new StringSlice(Slice.Text, Start, End) };

		// Make the parser jump over this tag when it continues parsing its data stream
		Slice.Start = End + 2;

		return true;
	}
}

