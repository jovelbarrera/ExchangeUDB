using System;
namespace Exchange.BranchService
{
	public class DeepLinkMetadata : IDeepLinkMetadata
	{
		public string Description { get; set; }

		public string ImageUrl { get; set; }

		public string Title { get; set; }

		public DeepLinkMetadata() { }

		public DeepLinkMetadata(string title, string description, string imageUrl)
		{
			Title = title;
			Description = description;
			ImageUrl = imageUrl;
		}
	}
}

