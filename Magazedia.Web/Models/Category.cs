namespace WikiWikiWorld.Models
{
	public class Category
	{
		public string Title { get; set; }
		public string? UrlSlug { get; set; }
		public PriorityOptions Priority { get; set; } 

		public enum PriorityOptions
		{
			Primary,
			Secondary
		};

		public Category(string Title, PriorityOptions Priority = PriorityOptions.Primary)
		{
			this.Title = Title;
			this.Priority = Priority;
		}

		public Category(string Title, string UrlSlug, PriorityOptions Priority = PriorityOptions.Primary) : this(Title, Priority) 
		{
			this.UrlSlug = UrlSlug;
		}
	}
}
