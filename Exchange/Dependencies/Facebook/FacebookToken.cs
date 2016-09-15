using System;

namespace Exchange.Dependencies.Facebook
{
	public class FacebookToken
	{
		public string UserId { get; set; }
		public string AccessToken { get; set; }
		public DateTime TokenExpiration { get; set; }
		public string[] GrantedPermissions { get; set; }
	}
}

