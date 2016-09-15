using System.Threading.Tasks;
using Exchange.Dependencies.Facebook;

namespace Exchange.Interfaces
{
	public interface IAuthProvider
	{
		Task SignUp(string email, string password);
		Task LogIn(FacebookToken facebookAccessToken);
		Task LogIn(string email, string password);
		Task ResetPassword(string email);
	}
}

