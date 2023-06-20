using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WikiWikiWorld.Models;
using Microsoft.Data.SqlClient;
using Dapper;
using System.Globalization;
using Markdig;
using MarkdigMantisLink;
using WikiWikiWorld.MarkdigExtensions;

namespace Magazedia.Web.Pages
{
	public class IndexModel : BasePageModel
	{
		public string? ArticleText { get; set; }
		public IndexModel(IConfiguration Configuration, IHttpContextAccessor HttpContextAccessor) : base(Configuration, HttpContextAccessor) { }


		public IEnumerable<WikiWikiWorld.Models.Article>? Articles { get; set; }


		public IActionResult OnGet()
		{
			using var Connection = new SqlConnection(Configuration.GetConnectionString("DefaultConnection"));
			int SiteId = 1;
			string SqlQuery = @"SELECT		*
								FROM		Articles
								WHERE		SiteId = @SiteId AND
											Culture = @Culture AND
											DateDeleted IS NULL
								ORDER BY	DateCreated DESC
								";

			Articles = Connection.Query<WikiWikiWorld.Models.Article>(SqlQuery, new { SiteId, Culture });

			ImageExtension ImageExtension = new(SiteId);
			ShortDescriptionExtension ShortDescriptionExtension = new(this);

			List<WikiWikiWorld.Models.Citation> CitationList = new();
			CitationExtension CitationExtension = new(CitationList);
			CitationsExtension CitationsExtension = new(CitationList);

			List<WikiWikiWorld.Models.Footnote> FootnoteList = new();
			FootnoteExtension FootnoteExtension = new(FootnoteList);
			FootnotesExtension FootnotesExtension = new(FootnoteList);

			List<WikiWikiWorld.Models.Category> CategoryList = new();
			CategoryExtension CategoryExtension = new(CategoryList);
			CategoriesExtension CategoriesExtension = new(CategoryList);
			StubExtension StubExtension = new(CategoryList);

			var pipeline = new	MarkdownPipelineBuilder()
								.UseAdvancedExtensions()
								.UseMantisLinks(new MantisLinkOptions("https://issues.company.net/"))
								.Use(ImageExtension)
								.Use(ShortDescriptionExtension)
								.Use(CategoryExtension)
								.Use(CategoriesExtension)
								.Use(StubExtension)
								.Use(CitationExtension)
								.Use(CitationsExtension)
								.Use(FootnoteExtension)
								.Use(FootnotesExtension)
								.Build();
			ArticleText = Markdown.ToHtml("hello", pipeline);

			return Page();
		}
	}
}
