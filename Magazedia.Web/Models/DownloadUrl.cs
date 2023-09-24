namespace WikiWikiWorld.Models
{
	public class DownloadUrl
	{
		public int Id { get; set; }
		public int SiteId { get; set; }
		public string Filename { get; set; }
		public string MimeType { get; set; }
		public int FileSizeBytes { get; set; }
		public byte[] HashSha256 { get; set; }
		public string DownloadUrls { get; set; }
		public bool NeedsOcr { get; set; }
		public bool IsComplete { get; set; }
		public string CreatedByAspNetUserId { get; set; }
		public DateTime DateCreated { get; set; }
		public DateTime DateModified { get; set; }
		public DateTime DateDeleted { get; set; }

		public DownloadUrl(int Id, int SiteId, string Filename, string MimeType, int FileSizeBytes, byte[] HashSha256, string DownloadUrls, bool NeedsOcr, bool IsComplete, string CreatedByAspNetUserId, DateTime DateCreated, DateTime DateModified, DateTime DateDeleted)
		{
			this.Id = Id;
			this.SiteId = SiteId;
			this.Filename = Filename;
			this.MimeType = MimeType;
			this.FileSizeBytes = FileSizeBytes;
			this.HashSha256 = HashSha256;
			this.DownloadUrls = DownloadUrls;
			this.NeedsOcr = NeedsOcr;
			this.IsComplete = IsComplete;
			this.CreatedByAspNetUserId = CreatedByAspNetUserId;
			this.DateCreated = DateCreated;
			this.DateModified = DateModified;
			this.DateDeleted = DateDeleted;
		}
	}
}
