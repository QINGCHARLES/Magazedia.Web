using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;
using System.Globalization;

namespace Magazedia.Web.Pages
{
    public class CultureSelectModel : BasePageModel
    {
        public string[] Cultures = { "en", "eu", "ru", "fr", "es", "ja", "ar" };
        public Dictionary<string, string> CultureCount = new ();

        public CultureSelectModel(IConfiguration Configuration, IHttpContextAccessor HttpContextAccessor) : base(Configuration, HttpContextAccessor) { }

        public void OnGet()
        {
            string SqlQuery = @"
                SELECT Culture, COUNT(*) as Count
                FROM Articles
                WHERE SiteId = @SiteId AND DateDeleted IS NULL
                GROUP BY Culture
            ";

            using SqlConnection Connection = new(Configuration.GetConnectionString("DefaultConnection"));
            int SiteId = 1;

            Dictionary<string, int> CulturesArticlesCounts = Connection.Query(SqlQuery, new { SiteId }).ToDictionary(x => (string)x.Culture, x => (int)x.Count);

            foreach (string Culture in Cultures)
            {
                int ArticleCount = CulturesArticlesCounts.ContainsKey(Culture) ? CulturesArticlesCounts[Culture] : 0;

                // Create a new NumberFormatInfo object
                NumberFormatInfo NumberFormatInfo = new CultureInfo("en-US", false).NumberFormat;

                // Set the thousand separator to a space
                NumberFormatInfo.NumberGroupSeparator = " ";

                // Use the custom format info with the ToString method
                string FormattedArticleCount = ArticleCount.ToString("#,0", NumberFormatInfo);

                CultureCount[Culture] = FormattedArticleCount;
            }
        }
    }
}
