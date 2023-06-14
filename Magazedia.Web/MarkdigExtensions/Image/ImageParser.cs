using Markdig.Helpers;
using Markdig.Parsers;
using MarkdigMantisLink;

namespace WikiWikiWorld.MarkdigExtensions;

public class ImageParser : InlineParser
{
	public ImageParser()
	{
		this.OpeningCharacters = new[] { '{' };
	}

	public override bool Match(InlineProcessor processor, ref StringSlice slice)
	{
		bool matchFound;
		char previous;

		matchFound = false;

		previous = slice.PeekCharExtra(-1);


		char current;
		int start;
		int end;

		slice.NextChar(); // skip the # starting character
		current = slice.CurrentChar;
		if (current != '{') return false;
		if (slice.NextChar() != 'I') return false;
		if (slice.NextChar() != 'm') return false;
		if (slice.NextChar() != 'a') return false;
		if (slice.NextChar() != 'g') return false;
		if (slice.NextChar() != 'e') return false;
		if (slice.NextChar() != ' ') return false;

		slice.NextChar(); // skip the # starting character

		current = slice.CurrentChar;

		start = slice.Start;
		end = start;



		// read as many digits as we can find, incrementing the slice length as we go
		while (current != '}')
		{
			end = slice.Start;
			current = slice.NextChar();
		}
		if (slice.NextChar() != '}') return false;
		slice.NextChar();
		// once we've ran out of digits, check to see what the next character is
		// to make sure this is a valid issue and nothing something random like #001Alpha
		if (current.IsWhiteSpaceOrZero() || current == ')' || current == '}')
		{
			int inlineStart;

			inlineStart = processor.GetSourcePosition(slice.Start, out int line, out int column);

			// and if we got here, then we've got a valid reference, so create our AST node
			// and assign it to the processor
			processor.Inline = new Image
			{
				Span =
							   {
								 Start = inlineStart,
								 End = inlineStart + (end - start) + 1 // add one to the length to account for the # starting character
                               },
				Line = line,
				Column = column,
				Data = new StringSlice(slice.Text, start, end)
			};

			matchFound = true;
		}

		return matchFound;
	}
}

