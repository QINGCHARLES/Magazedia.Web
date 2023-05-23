using System.ComponentModel.DataAnnotations;

namespace WikiWikiWorld.Models;

public class Article
{
	[Key]
	public int Id { get; set; }
	public int SiteId { get; set; }
	public string Culture { get; set; }
	public string Title { get; set; }
	public string UrlSlug { get; set; }
    public DateTime DateCreated { get; set; }
	public DateTime? DateDeleted { get; set; }

	public Article(int Id, int SiteId, string Culture, string Title, string UrlSlug, DateTime DateCreated, DateTime DateDeleted )
	{
		this.Id = Id;
		this.SiteId = SiteId;
		this.Culture = Culture;
		this.Title = Title;
		this.UrlSlug = UrlSlug;
		this.DateCreated = DateCreated;
		this.DateDeleted = DateDeleted;
	}

  //  public Article(int Id, int SiteId, string Language, string Title, string UrlSlug, string Text, string RevisionReason, string CreatedByAspNetUserId, DateTime DateCreated, DateTime DateDeleted, string CreatedByAspNetUsername)
		//		: this(Id, SiteId, Language, Title, UrlSlug, Text, RevisionReason, CreatedByAspNetUserId, DateCreated, DateDeleted)
  //  {
		//this.CreatedByAspNetUsername = CreatedByAspNetUsername;
  //  }
}
