using Markdig.Parsers;
using Markdig.Syntax;
using Markdig;

namespace WikiWikiWorld.MarkdigExtensions.MagazineInfoBox;

public class MagazineInfobox : ContainerBlock
{
	public Dictionary<string, string> Attributes { get; set; }

	public MagazineInfobox(BlockParser Parser) : base(Parser)
	{
		Attributes = new Dictionary<string, string>();
	}
}

public class MagazineInfoboxParser : BlockParser
{
	public MagazineInfoboxParser()
	{
		OpeningCharacters = new[] { '{' };
	}

	public override bool TryOpen(BlockProcessor Processor)
	{
		if (Processor.Line.Length < 19 || Processor.Line.CurrentChar != '{' || !Processor.Line.Match("{{MagazineInfobox "))
		{
			return false;
		}

		Processor.Line.Advance(18);

		string RemainingLine = Processor.Line.ToString();

		string[] Parameters = RemainingLine.TrimEnd('}', ' ').Split('|');

		MagazineInfobox MagazineInfoboxBlock = new MagazineInfobox(this);

		for (int ParameterIndex = 0; ParameterIndex < Parameters.Length; ParameterIndex += 2)
		{
			if (ParameterIndex + 1 < Parameters.Length && Parameters[ParameterIndex + 1] != "~")
			{
				MagazineInfoboxBlock.Attributes.Add(Parameters[ParameterIndex], Parameters[ParameterIndex + 1]);
			}
		}

		Processor.NewBlocks.Push(MagazineInfoboxBlock);

		return true;
	}
}

public class MagazineInfoboxRenderer : Markdig.Renderers.Html.HtmlObjectRenderer<MagazineInfobox>
{
	protected override void Write(Markdig.Renderers.HtmlRenderer Renderer, MagazineInfobox Obj)
	{
		Renderer.Write("<ul>");
		foreach (KeyValuePair<string, string> Attribute in Obj.Attributes)
		{
			Renderer
				.Write("<li>")
				.Write(Attribute.Key.ToLower() + ": ")
				.Write(Attribute.Value)
				.WriteLine("</li>");
		}
		Renderer.WriteLine("</ul>");
	}
}

public class MagazineInfoboxExtension : IMarkdownExtension
{
	public void Setup(MarkdownPipelineBuilder Pipeline)
	{
		Pipeline.BlockParsers.Insert(0, new MagazineInfoboxParser());

		if (!Pipeline.ObjectRenderers.Contains<MagazineInfoboxRenderer>())
		{
			Pipeline.ObjectRenderers.AddIfNotAlready<MagazineInfoboxRenderer>();
		}
	}

	public void Extend(MarkdownPipeline Pipeline, MarkdownDocument Document)
	{
	}
}
