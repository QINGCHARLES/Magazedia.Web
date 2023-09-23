using Markdig.Helpers;
using Markdig.Parsers;
using Microsoft.AspNetCore.Http;

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

        int barPosition = Slice.Text.IndexOf("|#|", Slice.Start);

		string urlSlug;
        Dictionary<string, string> attributes = new Dictionary<string, string>();

        if (barPosition > 0 && barPosition < End)
        {
            urlSlug = Slice.Text.Substring(Slice.Start, barPosition - Slice.Start);
            string attributesText = Slice.Text.Substring(barPosition + 1, End - barPosition - 1);
            string[] attributePairs = attributesText.Split("|#|");
            foreach (string attribute in attributePairs)
            {
                string[] parts = attribute.Split('=');
                if (parts.Length == 2)
                {
                    attributes[parts[0]] = parts[1];
                }
            }
        }
        else
        {
            urlSlug = Slice.Text.Substring(Slice.Start, End - Slice.Start);
        }

        Image imageInline = new Image
        {
            UrlSlug = urlSlug,
            Attributes = attributes
        };

        Processor.Inline = imageInline;
        Slice.Start = End + "}}".Length;

        return true;
    }
}
