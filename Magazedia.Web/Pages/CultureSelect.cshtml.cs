using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;

namespace Magazedia.Web.Pages
{
    public class CultureSelectModel : BasePageModel
    {
        public string[] Cultures = { "en", "eu", "ru", "fr", "es", "ja", "ar" };
        public Dictionary<string, int> CultureCount = new Dictionary<string, int>();

        public CultureSelectModel(IConfiguration Configuration, IHttpContextAccessor HttpContextAccessor) : base(Configuration, HttpContextAccessor)
        {
        }

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

            var results = Connection.Query(SqlQuery, new { SiteId }).ToDictionary(x => (string)x.Culture, x => (int)x.Count);

            foreach (var Culture in Cultures)
            {
                CultureCount[Culture] = results.ContainsKey(Culture) ? results[Culture] : 0;
            }
        }
    }
}
