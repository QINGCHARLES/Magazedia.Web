using System.ComponentModel.DataAnnotations;

namespace WikiWikiWorld.Models;
public class ArticleTalkSubject
{
	[Key]
	public int Id { get; set; }
	public int SiteId { get; set; }
	public int ArticleId { get; set; }
	public string Subject { get; set; }
	public string UrlSlug { get; set; }
	public bool HasBeenEdited { get; set; }
	public string CreatedByAspNetUserId { get; set; }
	public string? CreatedByAspNetUsername { get; set; }
	public List<ArticleTalkSubjectPost>? Posts { get; set; }
	public DateTime? DateNewestPost { get; set; }
	public DateTime DateCreated { get; set; }
	public DateTime? DateModified { get; set; }
	public DateTime? DateDeleted { get; set; }
	
	public ArticleTalkSubject(int Id, int SiteId, int ArticleId, string Subject, string UrlSlug, bool HasBeenEdited, string CreatedByAspNetUserId, DateTime DateCreated, DateTime DateModified, DateTime DateDeleted)
	{
		this.Id = Id;
		this.SiteId = SiteId;
		this.ArticleId = ArticleId;
		this.Subject = Subject;
		this.UrlSlug = UrlSlug;
		this.HasBeenEdited = HasBeenEdited;
		this.CreatedByAspNetUserId = CreatedByAspNetUserId;
		this.DateCreated = DateCreated;
		this.DateModified = DateModified;
		this.DateDeleted = DateDeleted;
	}

	public ArticleTalkSubject(int Id, int SiteId, int ArticleId, string Subject, string UrlSlug, bool HasBeenEdited, string CreatedByAspNetUserId, DateTime DateCreated, DateTime DateModified, DateTime DateDeleted, string CreatedByAspNetUsername)
					: this(Id, SiteId, ArticleId, Subject, UrlSlug, HasBeenEdited, CreatedByAspNetUserId, DateCreated, DateModified, DateDeleted)
	{
		this.CreatedByAspNetUsername = CreatedByAspNetUsername;
	}
}
