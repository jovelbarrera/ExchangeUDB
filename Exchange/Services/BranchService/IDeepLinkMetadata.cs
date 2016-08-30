using System;
namespace Exchange.BranchService
{
	public interface IDeepLinkMetadata
	{
		string Title { get; }
		string Description { get; }
		string ImageUrl { get; }
	}
}

