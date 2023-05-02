using Microsoft.Data.SqlClient;
using Dapper;

namespace WikiWikiWorld.Models;

public class Article
{
	public int Id { get; set; }
	public int SiteId { get; set; }
	public string Language { get; set; }
	public string Title { get; set; }
	public string UrlSlug { get; set; }
	public string Text { get; set; }
	public string RevisionReason { get; set; }
	public string CreatedByAspNetUserId { get; set; }
	public DateTime DateCreated { get; set; }
	public DateTime? DateDeleted { get; set; }
}
