using Markdig.Parsers;
using Markdig.Syntax;

namespace WikiWikiWorld.MarkdigExtensions;

//{{DownloadsBox Hash=f0e4c2f76c58916ec258f246851bea091d14d4247a2fc3e18694461b1816e13b|Description=A lovely thing.|Primary=https://www.yahoo.com/|Alternate=https://lycos.com/|Alternate=https://bbc.com/|#|Hash=ef6204f95e580471c2bf651f35d368616a61a7e577e36f7c40c896d176656e12|Description=A nice thing.|Primary=https://www.namecheap.com/|Alternate=https://spotify.com/|Alternate=https://youtube.com/}}


public class DownloadsBox : LeafBlock
{
	public List<Dictionary<string,string>> Downloads { get; }

	public DownloadsBox(BlockParser Parser, List<Dictionary<string, string>> Downloads) : base(Parser)
	{
		this.Downloads = Downloads;
	}
}