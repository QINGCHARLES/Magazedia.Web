using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Security.Policy;
using System.Text.RegularExpressions;
using WikiWikiWorld.Models;
using Microsoft.Data.SqlClient;
using Dapper;
using System.Configuration;

namespace Magazedia.Web.Pages.dev
{
	public class CoverListModel : BasePageModel
	{

		public class Cover
		{
			public string? Title { get; set; }
			public string? UrlSlug { get; set; }
		}


		public IList<Cover>? Covers { get; set; }


		public CoverListModel(IConfiguration Configuration, IHttpContextAccessor HttpContextAccessor) : base(Configuration, HttpContextAccessor) { }


		public void OnGet()
		{
			using var Connection = new SqlConnection(Configuration.GetConnectionString("DefaultConnection"));


			string sql = @"SELECT
    a.Title,
    a.UrlSlug
FROM
    Articles a
JOIN (
    SELECT
        ar.ArticleId,
        ar.[Text]
    FROM
        ArticleRevisions ar
    WHERE
        ar.Id = (
            SELECT TOP 1 ar2.Id
            FROM ArticleRevisions ar2
            WHERE ar2.ArticleId = ar.ArticleId
            ORDER BY ar2.DateCreated DESC
        )
) AS LatestArticleRevisions ON a.Id = LatestArticleRevisions.ArticleId
WHERE
    LatestArticleRevisions.[Text] LIKE '%Categories Magazines%'
    AND LatestArticleRevisions.[Text] LIKE '%magazine-cover-not-available%'
"
			;
			Covers = Connection.Query<Cover>(sql).ToList();

		}
	}
}
