using System.ComponentModel.DataAnnotations;

namespace WikiWikiWorld.Models;

public class ArticleRevision
{
	public int Id { get; set; }
	public string Title { get; set; }
	public string UrlSlug { get; set; }
	public string Text { get; set; }
	public string RevisionReason { get; set; }
	public DateTime DateCreated { get; set; }
	public string? CreatorUsername { get; set; }


	public ArticleRevision(int Id, string Title, string UrlSlug, string Text, string RevisionReason, DateTime DateCreated, string CreatorUsername)
	{
		this.Id = Id;
		this.Title = Title;
		this.UrlSlug = UrlSlug;
		this.Text = Text;
		this.RevisionReason = RevisionReason;
		this.DateCreated = DateCreated;
		this.CreatorUsername = CreatorUsername;
	}
}
