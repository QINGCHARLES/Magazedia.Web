namespace WikiWikiWorld.Models;

public class FileRevision
{
    public int Id { get; set; }
    public int ArticleId { get; set; }
    public string FileName { get; set; }
    public long FileSizeBytes { get; set; }
    public string MimeType { get; set; }
    public bool Is2dImage { get; set; }
    public bool IsVideo { get; set; }
    public bool IsAudio { get; set; }
    public string RevisionReason { get; set; }
    public string CreatedByAspNetUserId { get; set; }
    public string? CreatedByAspNetUsername { get; set; }
    public DateTime DateCreated { get; set; }
    public DateTime? DateDeleted { get; set; }

    public FileRevision(int Id, int ArticleId, string FileName, long FileSizeBytes, string MimeType, bool Is2dImage, bool IsVideo, bool IsAudio, string RevisionReason, string CreatedByAspNetUserId, DateTime DateCreated, DateTime DateDeleted)
    {
        this.Id = Id;
        this.ArticleId = ArticleId;
        this.FileName = FileName;
        this.FileSizeBytes = FileSizeBytes;
        this.MimeType = MimeType;
        this.Is2dImage = Is2dImage;
        this.IsVideo = IsVideo;
        this.IsAudio = IsAudio;
        this.RevisionReason = RevisionReason;
        this.CreatedByAspNetUserId = CreatedByAspNetUserId;
        this.DateCreated = DateCreated;
        this.DateDeleted = DateDeleted;
    }

    public FileRevision(int Id, int ArticleId, string FileName, long FileSizeBytes, string MimeType, bool Is2dImage, bool IsVideo, bool IsAudio, string RevisionReason, string CreatedByAspNetUserId, string CreatedByAspNetUsername, DateTime DateCreated, DateTime DateDeleted)
        : this(Id, ArticleId, FileName, FileSizeBytes, MimeType, Is2dImage, IsVideo, IsAudio, RevisionReason, CreatedByAspNetUserId, DateCreated, DateDeleted)
    {
        this.CreatedByAspNetUsername = CreatedByAspNetUsername;
    }
}
