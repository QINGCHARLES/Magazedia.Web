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
            foreach (var Culture in Cultures)
            {
                using SqlConnection Connection = new(Configuration.GetConnectionString("DefaultConnection"));
                int SiteId = 1;
                string SqlQuery = @"
                    SELECT COUNT(*)
                    FROM Articles
                    WHERE SiteId = @SiteId AND
                          Culture = @Culture AND
                          DateDeleted IS NULL
                ";

                int articleCount = Connection.QuerySingle<int>(SqlQuery, new { SiteId, Culture });
                CultureCount[Culture] = articleCount;
            }
        }
    }
}
