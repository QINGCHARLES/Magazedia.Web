using System.ComponentModel.DataAnnotations;

namespace WikiWikiWorld.Models;
public class ArticleCultureLink
{
	public string Title { get; set; }
	public string UrlSlug { get; set; }
	public string Culture { get; set; }

	public ArticleCultureLink(string Title, string UrlSlug, string Culture)
	{
		this.Title = Title;
		this.UrlSlug = UrlSlug;
		this.Culture = Culture;
	}
}
