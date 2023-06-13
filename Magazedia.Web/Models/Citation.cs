namespace WikiWikiWorld.Models
{
	public class Citation
	{
		public string Text { get; set; }
		public string? Name { get; set; }

		public Citation(string Text)
		{
			this.Text = Text;
		}

		public Citation(string Text, string Name) : this(Text) 
		{
			this.Name = Name;
		}
	}
}
