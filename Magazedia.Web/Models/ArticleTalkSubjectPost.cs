using System.ComponentModel.DataAnnotations;

namespace WikiWikiWorld.Models;
public class ArticleTalkSubjectPost
{
	[Key]
	public int Id { get; set; }
	public int ArticleTalkSubjectId { get; set; }
	public int? ParentTalkSubjectPostId { get; set; }
	public string Text { get; set; }
	public bool HasBeenEdited { get; set; }
	public string CreatedByAspNetUserId { get; set; }
	public DateTime DateCreated { get; set; }
	public DateTime? DateModified { get; set; }
	public DateTime? DateDeleted { get; set; }
	public string? ArticleTitle { get; set; }
	public string? TalkSubject { get; set; }
	public string? CreatedByAspNetUsername { get; set; }

	public ArticleTalkSubjectPost(int Id, int ArticleTalkSubjectId, int ParentTalkSubjectPostId, string Text, bool HasBeenEdited, string CreatedByAspNetUserId, DateTime DateCreated, DateTime DateModified, DateTime DateDeleted)
	{
		this.Id = Id;
		this.ArticleTalkSubjectId = ArticleTalkSubjectId;
		this.ParentTalkSubjectPostId = ParentTalkSubjectPostId;
		this.Text = Text;
		this.HasBeenEdited = HasBeenEdited;
		this.CreatedByAspNetUserId = CreatedByAspNetUserId;
		this.DateCreated = DateCreated;
		this.DateModified = DateModified;
		this.DateDeleted = DateDeleted;
	}

	public ArticleTalkSubjectPost(int Id, int ArticleTalkSubjectId, int ParentTalkSubjectPostId, string Text, bool HasBeenEdited, string CreatedByAspNetUserId, DateTime DateCreated, DateTime DateModified, DateTime DateDeleted, string ArticleTitle, string TalkSubject, string CreatedByAspNetUsername)
					: this(Id, ArticleTalkSubjectId, ParentTalkSubjectPostId, Text, HasBeenEdited, CreatedByAspNetUserId, DateCreated, DateModified, DateDeleted)
	{
		this.ArticleTitle = ArticleTitle;
		this.TalkSubject = TalkSubject;
		this.CreatedByAspNetUsername = CreatedByAspNetUsername;
	}

}
