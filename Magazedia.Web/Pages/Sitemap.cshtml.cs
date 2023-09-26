using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Security.Policy;
using System.Text;
using static Magazedia.Web.Pages.IndexModel;
using System.Text.RegularExpressions;
using Microsoft.Data.SqlClient;
using static Magazedia.Web.Pages.SitemapModel;
using System;

namespace Magazedia.Web.Pages
{
	public class SitemapModel : BasePageModel
	{
		public SitemapModel(IConfiguration Configuration, IHttpContextAccessor HttpContextAccessor) : base(Configuration, HttpContextAccessor) { }
		public class SiteMapArticle
		{
			public string UrlSlug { get; set; }
			public DateTime DateCreated { get; set; }

			public SiteMapArticle(string UrlSlug, DateTime DateCreated)
			{
				this.UrlSlug = UrlSlug;
				this.DateCreated = DateCreated;
			}
		}

		public IActionResult OnGet()
		{
			using SqlConnection Connection = new SqlConnection(Configuration.GetConnectionString("DefaultConnection"));
			int SiteId = 1;

			string SqlQuery = @"WITH LatestRevisions AS
								(
									SELECT ArticleId, MAX(DateCreated) AS MaxDateCreated
									FROM ArticleRevisions
									WHERE DateDeleted IS NULL
									GROUP BY ArticleId
								)

								SELECT Articles.UrlSlug, ArticleRevisions.DateCreated
								FROM Articles
								JOIN ArticleRevisions ON Articles.Id = ArticleRevisions.ArticleId
								JOIN LatestRevisions ON ArticleRevisions.ArticleId = LatestRevisions.ArticleId AND ArticleRevisions.DateCreated = LatestRevisions.MaxDateCreated
								WHERE Articles.SiteId = @SiteId
								AND Articles.Culture = @Culture
								AND Articles.DateDeleted IS NULL
								ORDER BY ArticleRevisions.DateCreated DESC;
								";


			IEnumerable<SiteMapArticle>? SiteMapArticles = Connection.Query<SiteMapArticle>(SqlQuery, new { SiteId, Culture });

			StringBuilder sb = new StringBuilder();
			sb.Append("<?xml version='1.0' encoding='UTF-8' ?><urlset xmlns = 'http://www.sitemaps.org/schemas/sitemap/0.9'>");

			foreach(SiteMapArticle Article in SiteMapArticles)
			{
				string LastModifiedDate = DateTime.SpecifyKind(Article.DateCreated, DateTimeKind.Utc).ToString("yyyy-MM-ddTHH:mm:ssK");
				sb.Append($"<url><loc>{Request.Scheme}://{Request.Host}/{Article.UrlSlug}</loc><lastmod>{LastModifiedDate}</lastmod></url>");
			}

			sb.Append("</urlset>");

			return new ContentResult
			{
				ContentType = "application/xml",
				Content = sb.ToString(),
				StatusCode = 200
			};
		}
	}
}
