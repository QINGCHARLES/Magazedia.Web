namespace WikiWikiWorld.Models
{
	public class Download
	{
		public string Filename { get; set; }
		public string FileType { get; set; }
		public string SiteName { get; set; }
		public string Url { get; set; }
		public int FileSizeBytes { get; set; }
		public bool PrimarySite { get; set; }

		public Download(string Filename, string FileType, string SiteName, string Url, int FileSizeBytes, bool PrimarySite)
		{
			this.Filename = Filename;
			this.FileType = FileType;
			this.SiteName = SiteName;
			this.Url = Url;
			this.FileSizeBytes = FileSizeBytes;
			this.PrimarySite = PrimarySite;
		}
	}
}
