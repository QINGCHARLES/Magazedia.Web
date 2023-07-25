using Markdig.Helpers;
using Markdig.Parsers;

namespace WikiWikiWorld.MarkdigExtensions;

public class ImageParser : InlineParser
{
	const string Tag = "Image";

	public ImageParser()
	{
		OpeningCharacters = new[] { '{' };
	}

	public override bool Match(InlineProcessor Processor, ref StringSlice Slice)
	{
		if (!Slice.Match("{{" + Tag + " ")) return false;

		int Start = Slice.Start;
		Slice.Start += Tag.Length + 3; // 3 = {{ + space

		int End = Slice.IndexOf("}}");
		if (End == -1) return false;

		Processor.Inline = new Image { Data = new StringSlice(Slice.Text, Start, End) };

		// Make the parser jump over the {{Tag ..}} when it continues parsing its data stream
		Slice.Start = End + 2;

		return true;
	}
}

