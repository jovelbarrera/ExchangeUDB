using System;
using System.Threading.Tasks;

namespace Exchange.Dependencies.Facebook
{
	public interface IFacebookButton
	{
		void LoginWithReadPermissions (string[] permissions, Action<FacebookEvent> callback);
		void LoginWithWritePermissions (string[] permissions, Action<FacebookEvent> callback);
		void Logout();
	}
}

