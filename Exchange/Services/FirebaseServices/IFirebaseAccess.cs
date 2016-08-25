using System;

namespace Exchange.Services.FirebaseServices
{
	public interface IFirebaseAccess
	{
		string FirebaseBasePath { get; }
		string ApiKey { get; }
	}
}

