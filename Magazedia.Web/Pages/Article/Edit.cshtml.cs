using Markdig;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;
using Dapper;
using WikiWikiWorld.Models;
using System.Security.Claims;
using System.Text.RegularExpressions;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Security.Policy;
using Markdig.Parsers;
using WikiWikiWorld.MarkdigExtensions;

namespace Magazedia.Web.Pages;
public class EditModel : BasePageModel
{
	public string? ArticleTitle { get; set; }
	public string? ArticleUrlSlug { get; set; }
	public string? ArticleHtml { get; set; }
	public string? ArticleRevisionReason { get; set; }
	public string? ArticleText { get; set; }

	public WikiWikiWorld.Models.ArticleRevision? ArticleRevision { get; set; }

	[BindProperty(SupportsGet = true)]
	public string? UrlSlug { get; set; }

	public EditModel(IConfiguration Configuration, IHttpContextAccessor HttpContextAccessor) : base(Configuration, HttpContextAccessor) { }

	public IActionResult OnPost()
	{
		var specificUsername = "QINGCHARLES";

		if (this.User == null || this.User.Identity == null || !this.User.Identity.IsAuthenticated || (this.User.Identity.IsAuthenticated && this.User.Identity.Name != specificUsername))
		{

			return BadRequest();
		}

		ArticleTitle = Request.Form[nameof(ArticleTitle)];
		ArticleText = Request.Form[nameof(ArticleText)];
		ArticleUrlSlug = Request.Form[nameof(ArticleUrlSlug)];
		ArticleRevisionReason = Request.Form[nameof(ArticleRevisionReason)];

		int SiteId = 1;

		ClaimsPrincipal? User = HttpContextAccessor.HttpContext?.User;
		string Username = User?.FindFirstValue(ClaimTypes.NameIdentifier) ?? "7240be61-df81-46f9-8152-6a48b96abc40";// "Anonymous";

		using var Connection = new SqlConnection(Configuration.GetConnectionString("DefaultConnection"));

		string SqlQuery = "SELECT * FROM Articles WHERE UrlSlug = @UrlSlug AND Culture = @Culture AND SiteId = @SiteId AND DateDeleted IS NULL";
		var Article = Connection.QuerySingleOrDefault(SqlQuery, new { UrlSlug = ArticleUrlSlug, SiteId, Culture });


		SqlQuery = @"	INSERT ArticleRevisions ([ArticleId], [Text], [RevisionReason], [CreatedByAspNetUserId])
							VALUES (@ArticleId, @Text, @RevisionReason, @CreatedByAspNetUserId);
						";
		var res = Connection.Execute(SqlQuery, new { ArticleId = Article.Id, Text = ArticleText, RevisionReason = ArticleRevisionReason, CreatedByAspNetUserId = Username });



		return Content("output:" + res.ToString() + UrlSlug + ArticleText);
	}
	public IActionResult OnGet()
	{
		//if (this.User != null && this.User.Identity != null)
		//{
		//	return Content(this.User!.Identity!.Name!);
		//}

		this.AllowSearchEngineIndexing = false;

		using var Connection = new SqlConnection(Configuration.GetConnectionString("DefaultConnection"));
		int SiteId = 1;
		string SqlQuery = @"
							SELECT		ar.Id, ar.ArticleId, a.Title, a.UrlSlug, ar.[Text], ar.RevisionReason, ar.CreatedByAspNetUserId, u.UserName as CreatedByAspNetUsername, ar.DateCreated, ar.DateDeleted
							FROM		Articles a
							INNER JOIN	ArticleRevisions ar ON a.Id = ar.ArticleId
							INNER JOIN	AspNetUsers u ON ar.CreatedByAspNetUserId = u.Id
							WHERE		a.UrlSlug = @UrlSlug AND
										a.SiteId = @SiteId AND
										a.Culture = @Culture AND
										a.DateDeleted IS NULL AND
										ar.DateDeleted IS NULL AND
										ar.DateCreated =
										(
											SELECT	MAX(DateCreated)
											FROM	ArticleRevisions
											WHERE	ArticleId = a.Id
										);
						";

		//SELECT TOP(1) * FROM Articles WHERE UrlSlug = @UrlSlug AND Language = @Language AND DateDeleted IS NULL ORDER BY DateCreated DESC";
		ArticleRevision = Connection.QuerySingleOrDefault<ArticleRevision>(SqlQuery, new { UrlSlug, SiteId, Culture });

		//Article = Connection.QuerySingleOrDefault<Article>(SqlQuery, new { UrlSlug = UrlSlug, Language = Language });

		if (ArticleRevision is null)
		{
			return NotFound();
		}


		//	TestSpec("https://example.com", "Image: ![alt text](/image.jpg)", "https://example.com/image.jpg");
		//	TestSpec("https://example.com", "Image: ![alt text](image.jpg \"title\")", "https://example.com/image.jpg");
		//	TestSpec(null, "Image: ![alt text](/image.jpg)", "/image.jpg");
		//}

		//public static void TestSpec(string baseUrl, string markdown, string expectedLink)
		//{

		//	var pipeline = new MarkdownPipelineBuilder().Build();
		ImageExtension ImageExtension = new ImageExtension(SiteId, Connection);
		MarkdownPipeline Pipeline = new MarkdownPipelineBuilder().UseAdvancedExtensions().Use(ImageExtension).Build();

		var writer = new StringWriter();
		var renderer = new Markdig.Renderers.HtmlRenderer(writer);
		renderer.BaseUrl = new Uri($"https://{HttpContextAccessor.HttpContext!.Request.Host.Value}/");
		Pipeline.Setup(renderer);

		ArticleTitle = ArticleRevision.Title;
		ArticleUrlSlug = ArticleRevision.UrlSlug;
		ArticleText = ArticleRevision.Text;

		var document = MarkdownParser.Parse(ArticleRevision.Text, Pipeline);
		renderer.Render(document);
		writer.Flush();
		ArticleHtml = writer.ToString(); // Markdown.ToHtml(ArticleRevision.Text, Pipeline);

		return Page();
	}


}

