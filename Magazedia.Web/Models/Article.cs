using System.ComponentModel.DataAnnotations;

namespace WikiWikiWorld.Models;

public class Article
{
	[Key]
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

	public Article(int Id, int SiteId, string Language, string Title, string UrlSlug, string Text, string RevisionReason, string CreatedByAspNetUserId, DateTime DateCreated )
	{
		this.Id = Id;
		this.SiteId = SiteId;
		this.Language = Language;
		this.Title = Title;
		this.UrlSlug = UrlSlug;
		this.Text = Text;
		this.RevisionReason = RevisionReason;
		this.CreatedByAspNetUserId = CreatedByAspNetUserId;
		this.DateCreated = DateCreated;
	}
}
