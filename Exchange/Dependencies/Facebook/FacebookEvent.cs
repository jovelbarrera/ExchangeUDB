using System;
using System.Threading.Tasks;

namespace Exchange.Dependencies.Facebook
{
	public class FacebookEvent
	{
		public string UserId { get; set; }
		public string AccessToken { get; set; }
		public DateTime TokenExpiration { get; set; }
		public string[] GrantedPermissions { get; set; }
	}
}

