using System.ComponentModel.DataAnnotations;

namespace WikiWikiWorld.Models;
public class ArticleTalkSubject
{
	[Key]
	public int Id { get; set; }
	public int SiteId { get; set; }
	public string Culture { get; set; }
	public string ArticleTitle { get; set; }
	public string Subject { get; set; }
	public string UrlSlug { get; set; }
	public string Text { get; set; }
	public bool HasBeenEdited { get; set; }
	public string CreatedByAspNetUserId { get; set; }
	public string? CreatedByAspNetUsername { get; set; }
	public List<ArticleTalkSubjectPost>? Posts { get; set; }
	public DateTime? DateNewestPost { get; set; }
	public DateTime DateCreated { get; set; }
	public DateTime? DateModified { get; set; }
	public DateTime? DateDeleted { get; set; }
	
	public ArticleTalkSubject(int Id, int SiteId, string Culture, string ArticleTitle, string Subject, string UrlSlug, string Text, bool HasBeenEdited, string CreatedByAspNetUserId, DateTime DateCreated, DateTime DateModified, DateTime DateDeleted)
	{
		this.Id = Id;
		this.SiteId = SiteId;
		this.Culture = Culture;
		this.ArticleTitle = ArticleTitle;
		this.Subject = Subject;
		this.UrlSlug = UrlSlug;
		this.Text = Text;
		this.HasBeenEdited = HasBeenEdited;
		this.CreatedByAspNetUserId = CreatedByAspNetUserId;
		this.DateCreated = DateCreated;
		this.DateModified = DateModified;
		this.DateDeleted = DateDeleted;
	}

	public ArticleTalkSubject(int Id, int SiteId, string Culture, string ArticleTitle, string Subject, string UrlSlug, string Text, bool HasBeenEdited, string CreatedByAspNetUserId, DateTime DateCreated, DateTime DateModified, DateTime DateDeleted, string CreatedByAspNetUsername)
					: this(Id, SiteId, Culture, ArticleTitle, Subject, UrlSlug, Text, HasBeenEdited, CreatedByAspNetUserId, DateCreated, DateModified, DateDeleted)
	{
		this.CreatedByAspNetUsername = CreatedByAspNetUsername;
	}
}
