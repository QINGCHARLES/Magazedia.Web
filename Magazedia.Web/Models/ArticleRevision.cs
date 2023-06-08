using System.ComponentModel.DataAnnotations;

namespace WikiWikiWorld.Models;

public class ArticleRevision
{
	public int Id { get; set; }
	public int ArticleId { get; set; }
	public string? Title { get; set; }
	public string? UrlSlug { get; set; }
	public string Text { get; set; }
	public string RevisionReason { get; set; }
	public string CreatedByAspNetUserId { get; set; }
	public string? CreatedByAspNetUsername { get; set; }
	public DateTime DateCreated { get; set; }
	public DateTime? DateModified { get; set; }
	public DateTime? DateDeleted { get; set; }

	public ArticleRevision(int Id, int ArticleId, string Text, string RevisionReason, string CreatedByAspNetUserId, DateTime DateCreated, DateTime DateModified, DateTime DateDeleted)
	{
		this.Id = Id;
		this.ArticleId = ArticleId;
		this.Text = Text;
		this.RevisionReason = RevisionReason;
		this.CreatedByAspNetUserId = CreatedByAspNetUserId;
		this.DateCreated = DateCreated;
		this.DateModified = DateModified;
		this.DateDeleted = DateDeleted;
	}

	public ArticleRevision(int Id, int ArticleId, string Title, string UrlSlug, string Text, string RevisionReason, string CreatedByAspNetUserId, string CreatedByAspNetUsername, DateTime DateCreated, DateTime DateModified, DateTime DateDeleted)
		: this( Id, ArticleId, Text, RevisionReason, CreatedByAspNetUserId, DateCreated, DateModified, DateDeleted)
	{
		this.Title = Title;
		this.UrlSlug = UrlSlug;
		this.CreatedByAspNetUsername = CreatedByAspNetUsername;
	}



}
