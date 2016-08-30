using System;
using System.Collections.Generic;

namespace Exchange.BranchService
{
	public interface IDeepLinkService<I,T>
	{
		void ReadLink(Dictionary<string, object> data);
		void GenerateLink(Dictionary<string, object> data, IDeepLinkMetadata deepLinkMetadata);
	}
}

