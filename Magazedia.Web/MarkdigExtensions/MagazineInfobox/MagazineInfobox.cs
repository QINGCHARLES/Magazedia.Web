using Markdig.Parsers;
using Markdig.Syntax;
using Markdig.Renderers;
using Markdig.Renderers.Html;
using Markdig;

// Define your custom MagazineInfobox
public class MagazineInfobox : LeafBlock
{
	public MagazineInfobox(BlockParser Parser, Dictionary<string, string> Data) : base(Parser)
	{
		this.Data = Data;
	}

	public Dictionary<string, string> Data { get; }
}

// Define your custom MagazineInfoboxParser
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
		string[] Pairs = DataString.Split(new[] { "|~|" }, StringSplitOptions.None);
		Dictionary<string, string> Data = Pairs.Select(pair =>
		{
			string[] parts = pair.Split('|');
			return new { Var = parts[0], Text = parts[1] };
		}).ToDictionary(x => x.Var, x => x.Text);

		MagazineInfobox MagazineInfobox = new MagazineInfobox(this, Data);
		Processor.NewBlocks.Push(MagazineInfobox);

		return BlockState.ContinueDiscard;
	}
}

// Define your custom MagazineInfoboxHtmlRenderer
public class MagazineInfoboxHtmlRenderer : HtmlObjectRenderer<MagazineInfobox>
{
	protected override void Write(HtmlRenderer Renderer, MagazineInfobox Obj)
	{
		Renderer.Write("<aside class=\"infobox\"><ul>");
		foreach (KeyValuePair<string, string> pair in Obj.Data)
		{
			Renderer.Write("<li>").Write(pair.Key).Write(": ").Write(pair.Value).Write("</li>");
		}
		Renderer.Write("</ul></aside>");
	}
}

// Add your custom extension
public class MagazineInfoboxExtension : IMarkdownExtension
{
	public void Setup(MarkdownPipelineBuilder Pipeline)
	{
		Pipeline.BlockParsers.InsertBefore<ParagraphBlockParser>(new MagazineInfoboxParser());
	}

	public void Setup(MarkdownPipeline Pipeline, IMarkdownRenderer Renderer)
	{
		if (Renderer is HtmlRenderer HtmlRenderer)
		{
			HtmlRenderer.ObjectRenderers.Add(new MagazineInfoboxHtmlRenderer());
		}
	}
}
