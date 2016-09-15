using System;
using System.Threading.Tasks;
using Exchange.Dependencies.Facebook;
using Exchange.Interfaces;

namespace Exchange.Services.AuthService
{
	public abstract class AuthProvider<I, T> : IAuthProvider
		where T : IUser
	{
		private static I _instance;
		public static I Instance
		{
			get
			{
				if (_instance == null)
				{
					_instance = (I)Activator.CreateInstance(typeof(I));
				}

				return _instance;
			}
		}

		public abstract Task LogIn(string email, string password);

		public abstract Task LogIn(FacebookToken facebookToken);

		public abstract Task SignUp(string email, string password);

		public abstract Task ResetPassword(string email);
	}
}

