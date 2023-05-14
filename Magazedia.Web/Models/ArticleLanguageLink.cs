using System.ComponentModel.DataAnnotations;
using static System.Net.Mime.MediaTypeNames;

namespace WikiWikiWorld.Models;
public class ArticleLanguageLink
{
	[Key]
	public int Id { get; set; }
	public int SiteId { get; set; }
	public int ArticleLanguageGroupId { get; set; }
	public string Language { get; set; }
	public string ArticleTitle { get; set; }
	public DateTime? DateDeleted { get; set; }
	public string? ArticleUrlSlug { get; set; }

	public ArticleLanguageLink(int Id, int SiteId, int ArticleLanguageGroupId, string Language, string ArticleTitle, DateTime DateDeleted)
	{
		this.Id = Id;
		this.SiteId = SiteId;
		this.ArticleLanguageGroupId = ArticleLanguageGroupId;
		this.Language = Language;
		this.ArticleTitle = ArticleTitle;
		this.DateDeleted = DateDeleted;
	}

	public ArticleLanguageLink(int Id, int SiteId, int ArticleLanguageGroupId, string Language, string ArticleTitle, DateTime DateDeleted, string ArticleUrlSlug)
				: this(Id, SiteId, ArticleLanguageGroupId, Language, ArticleTitle, DateDeleted)
	{
		this.ArticleUrlSlug = ArticleUrlSlug;
	}
}
