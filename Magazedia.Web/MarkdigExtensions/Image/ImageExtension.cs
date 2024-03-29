﻿using Markdig;
using Markdig.Helpers;
using Markdig.Parsers;
using Markdig.Renderers;
using Microsoft.Data.SqlClient;

namespace WikiWikiWorld.MarkdigExtensions;

public class ImageExtension : IMarkdownExtension
{
	private readonly int SiteId;
	private readonly SqlConnection Connection;
	private readonly Magazedia.Web.Pages.BasePageModel Page;

	public ImageExtension(int SiteId, SqlConnection Connection, Magazedia.Web.Pages.BasePageModel Page)
	{
		this.SiteId = SiteId;
		this.Connection = Connection;
		this.Page = Page;
	}

	public void Setup(MarkdownPipelineBuilder pipeline)
	{
		OrderedList<InlineParser> parsers;

		parsers = pipeline.InlineParsers;

		if (!parsers.Contains<ImageParser>())
		{
			parsers.Add(new ImageParser());
		}
	}

	public void Setup(MarkdownPipeline pipeline, IMarkdownRenderer renderer)
	{
		HtmlRenderer? htmlRenderer;
		ObjectRendererCollection? renderers;

		htmlRenderer = renderer as HtmlRenderer;
		renderers = htmlRenderer?.ObjectRenderers;

		if (renderers != null && !renderers.Contains<ImageRenderer>())
		{
			renderers!.Add(new ImageRenderer(SiteId, Connection, Page));
		}
	}
}
