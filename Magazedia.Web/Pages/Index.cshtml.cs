using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WikiWikiWorld.Models;
using Microsoft.Data.SqlClient;
using Dapper;
using System.Globalization;
using Markdig;
using MarkdigMantisLink;
using WikiWikiWorld.MarkdigExtensions;
using System.Text.RegularExpressions;
using Microsoft.Extensions.FileSystemGlobbing.Internal;

namespace Magazedia.Web.Pages
{
	public class IndexModel : BasePageModel
	{
		public string? ArticleText { get; set; }
		public IndexModel(IConfiguration Configuration, IHttpContextAccessor HttpContextAccessor) : base(Configuration, HttpContextAccessor) { }

		public IEnumerable<WikiWikiWorld.Models.Article>? Articles { get; set; }

		public IEnumerable<MostRecentlyUpdatedMagazineArticle>? MostRecentlyUpdatedMagazineArticles { get; set; }
		public IEnumerable<MostRecentlyUpdatedMagazineArticle>? MostRecentlyUpdatedMagazineIssueArticles { get; set; }

		public class MostRecentlyUpdatedMagazineArticle
		{
			public string Title { get; set; }
			public string UrlSlug { get; set; }
			public string Text { get; set; }
			public string? PrimaryArticleImageUrl { get; set; }
			public string? PrimaryArticleImageTitle { get; set; }

			public MostRecentlyUpdatedMagazineArticle(string Title, string UrlSlug, string Text)
			{
				this.Title = Title;
				this.UrlSlug = UrlSlug;
				this.Text = Text;
			}
		}

		public IActionResult OnGet()
		{
			using SqlConnection Connection = new SqlConnection(Configuration.GetConnectionString("DefaultConnection"));
			int SiteId = 1;
			//    string SqlQuery = @"SELECT		*
			//FROM		Articles
			//WHERE		SiteId = @SiteId AND
			//			Culture = @Culture AND
			//			DateDeleted IS NULL
			//ORDER BY	DateCreated DESC
			//";

			//    Articles = Connection.Query<WikiWikiWorld.Models.Article>(SqlQuery, new { SiteId, Culture });

			string CategoriesTypeToMatch = "Magazines";
			string CategoriesTag = "Category";
			string CategoriesSearchTerm = "%{{" + CategoriesTag + " " + CategoriesTypeToMatch + "}}%";

			string SqlQuery = @"WITH LatestRevisions AS
								(
									SELECT ArticleId, MAX(DateCreated) AS MaxDateCreated
									FROM ArticleRevisions
									WHERE	([Text] LIKE @CategoriesSearchTerm AND [Text] LIKE '%Type=PrimaryArticleImage%')
											OR
											([Text] LIKE '%{{MagazineInfobox%' AND [Text] LIKE '%PrimaryCoverImageUrlSlug=%')
									AND DateDeleted IS NULL
									GROUP BY ArticleId
								)

								SELECT TOP(100) Articles.Title, Articles.UrlSlug, ArticleRevisions.[Text]
								FROM Articles
								JOIN ArticleRevisions ON Articles.Id = ArticleRevisions.ArticleId
								JOIN LatestRevisions ON ArticleRevisions.ArticleId = LatestRevisions.ArticleId AND ArticleRevisions.DateCreated = LatestRevisions.MaxDateCreated
								WHERE Articles.SiteId = @SiteId
								AND Articles.Culture = @Culture
								AND Articles.DateDeleted IS NULL
								ORDER BY ArticleRevisions.DateCreated DESC;
								";

			MostRecentlyUpdatedMagazineArticles = Connection.Query<MostRecentlyUpdatedMagazineArticle>(SqlQuery, new { SiteId, Culture, CategoriesSearchTerm });

			// For each magazine get the UrlSlug of the PrimaryImageArticle and then convert that UrlSlug into an actual Url for the image
			foreach (MostRecentlyUpdatedMagazineArticle MostRecentlyUpdatedMagazineArticle in MostRecentlyUpdatedMagazineArticles)
			{
				// Match the `PrimaryCoverImageUrlSlug` in either the `Image` or `MagazineInfobox` tags.
				string MatchPattern = @"{{Image (image:.+?)\|#\|Type=PrimaryArticleImage}}|{{MagazineInfobox PrimaryCoverImageUrlSlug=(image:.+?)\|#\|";

				MatchCollection matches = Regex.Matches(MostRecentlyUpdatedMagazineArticle.Text, MatchPattern, RegexOptions.IgnoreCase);

				if (matches.Count > 0) // Ensure there's at least one match
				{
					Match match = matches[0];

					// Determine which capturing group contains the desired URL slug.
					string imageLink = !string.IsNullOrEmpty(match.Groups[1].Value) ? match.Groups[1].Value : match.Groups[2].Value;

					(MostRecentlyUpdatedMagazineArticle.PrimaryArticleImageUrl, MostRecentlyUpdatedMagazineArticle.PrimaryArticleImageTitle) = Helpers.GetImageFilenameAndArticleTitleFromArticleUrlSlug(imageLink, Connection);
				}
			}

			CategoriesTypeToMatch = "Magazine issues";
			CategoriesSearchTerm = "%{{" + CategoriesTag + " " + CategoriesTypeToMatch + "}}%";


			SqlQuery = @"	WITH LatestRevisions AS
							(
								SELECT ArticleId, MAX(DateCreated) AS MaxDateCreated
								FROM ArticleRevisions
									WHERE	([Text] LIKE @CategoriesSearchTerm AND [Text] LIKE '%Type=PrimaryArticleImage%')
											OR
											([Text] LIKE '%{{MagazineInfobox%' AND [Text] LIKE '%PrimaryCoverImageUrlSlug=%')
								AND DateDeleted IS NULL
								GROUP BY ArticleId
							)

							SELECT Articles.Title, Articles.UrlSlug, ArticleRevisions.[Text]
							FROM Articles
							JOIN ArticleRevisions ON Articles.Id = ArticleRevisions.ArticleId
							JOIN LatestRevisions ON ArticleRevisions.ArticleId = LatestRevisions.ArticleId AND ArticleRevisions.DateCreated = LatestRevisions.MaxDateCreated
							WHERE Articles.SiteId = @SiteId
							AND Articles.Culture = @Culture
							AND Articles.DateDeleted IS NULL
							ORDER BY ArticleRevisions.DateCreated DESC;
							";

			MostRecentlyUpdatedMagazineIssueArticles = Connection.Query<MostRecentlyUpdatedMagazineArticle>(SqlQuery, new { SiteId, Culture, CategoriesSearchTerm });

			foreach (MostRecentlyUpdatedMagazineArticle MostRecentlyUpdatedMagazineArticle in MostRecentlyUpdatedMagazineIssueArticles)
			{
				string MatchPattern = @"{{Image (image:.+?)\|#\|Type=PrimaryArticleImage}}|{{MagazineInfobox PrimaryCoverImageUrlSlug=(image:.+?)\|#\|";

				MatchCollection matches = Regex.Matches(MostRecentlyUpdatedMagazineArticle.Text, MatchPattern, RegexOptions.IgnoreCase);
				Match match = matches[0];

				if (match.Groups.Count > 1) // Check if the desired capturing group exists
				{
					string imageLink = match.Groups[1].Value;
					(MostRecentlyUpdatedMagazineArticle.PrimaryArticleImageUrl, MostRecentlyUpdatedMagazineArticle.PrimaryArticleImageTitle) = Helpers.GetImageFilenameAndArticleTitleFromArticleUrlSlug(imageLink, Connection);
				}
			}

			//ImageExtension ImageExtension = new(SiteId);
			//ShortDescriptionExtension ShortDescriptionExtension = new(this);

			//List<WikiWikiWorld.Models.Citation> CitationList = new();
			//CitationExtension CitationExtension = new(CitationList);
			//CitationsExtension CitationsExtension = new(CitationList);

			//List<WikiWikiWorld.Models.Footnote> FootnoteList = new();
			//FootnoteExtension FootnoteExtension = new(FootnoteList);
			//FootnotesExtension FootnotesExtension = new(FootnoteList);

			//List<WikiWikiWorld.Models.Category> CategoryList = new();
			//CategoryExtension CategoryExtension = new(CategoryList);
			//CategoriesExtension CategoriesExtension = new(CategoryList);
			//StubExtension StubExtension = new(CategoryList);

			//var pipeline = new	MarkdownPipelineBuilder()
			//					.UseAdvancedExtensions()
			//					.UseMantisLinks(new MantisLinkOptions("https://issues.company.net/"))
			//					.Use(ImageExtension)
			//					.Use(ShortDescriptionExtension)
			//					.Use(CategoryExtension)
			//					.Use(CategoriesExtension)
			//					.Use(StubExtension)
			//					.Use(CitationExtension)
			//					.Use(CitationsExtension)
			//					.Use(FootnoteExtension)
			//					.Use(FootnotesExtension)
			//					.Build();
			//ArticleText = Markdown.ToHtml("hello", pipeline);

			return Page();
		}
	}
}
