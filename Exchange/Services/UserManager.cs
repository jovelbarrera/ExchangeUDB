using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Exchange.Models;
using Exchange.RealmServices;
using Exchange.Services.FirebaseServices;
using Firebase.Xamarin.Auth;
using System.Reflection;
using Exchange.Helpers;
using Exchange.Interfaces;
using Exchange.Configs;

namespace Exchange.Services
{
	public class UserManager
	{
		private static UserManager _instance;
		public static UserManager Instance
		{
			get
			{
				if (_instance == null)
					_instance = new UserManager();
				return _instance;
			}
		}

		public IUser CurrentUser
		{
			get { return GetCurrentUser(); }
			set { SetCurrentUser(value); }
		}

		private IUser GetCurrentUser()
		{
			List<IUser> users = RealmService.Instance.All<CurrentUser>().ToList<IUser>();
			IUser user = users.FirstOrDefault();
			return user;
		}

		private IUser SetCurrentUser(IUser newuser)
		{
			//string[] userProperties = {
			//	"ObjectId",
			//	"CreatedAt",
			//	"UpdatedAt",
			//	"Username",
			//	"DisplayName",
			//	"FirstName",
			//	"LastName",
			//	"Email",
			//	"University",
			//	"Career",
			//	"About",
			//	"ProfilePicture"
			//};

			PropertyInfo[] modelProperties = typeof(IModel).GetRuntimeProperties().ToArray();
			PropertyInfo[] userProperties = typeof(IUser).GetRuntimeProperties().ToArray();
			PropertyInfo[] properties = modelProperties.Concat(userProperties).ToArray();
			IUser user = CurrentUser;
			if (user == null)
			{
				RealmService.Instance.Write(() =>
				{
					user = RealmService.Instance.CreateObject<CurrentUser>();
					foreach (var property in properties)
					{
						try
						{
							object val = newuser.GetType().GetRuntimeProperty(property.Name).GetValue(newuser);
							user.GetType().GetRuntimeProperty(property.Name).SetValue(user, val);
						}
						catch (Exception ex)
						{
							var a = ex.Message;
						}
					}
				});
			}
			else
			{
				using (var trans = RealmService.Instance.BeginWrite())
				{
					user = newuser;
					trans.Commit();
				}
			}
			return user;
		}

		public async Task SignUpWithCredentials(string email, string password, Action<bool> continueWith)
		{
			await FirebaseAuthService.Factory(FirebaseAccess.Instance).SignUpWithCredentials(email, password, new AuthCallback(continueWith));
		}

		public async Task SignInWithFacebook(string facebookAccessToken, Action<bool> continueWith)
		{
			await FirebaseAuthService.Factory(FirebaseAccess.Instance).SignInWithFacebook(facebookAccessToken, new AuthCallback(continueWith));
		}

		public async Task SignInWithCredentials(string email, string password, Action<bool> continueWith)
		{
			await FirebaseAuthService.Factory(FirebaseAccess.Instance).SignInWithCredentials(email, password, new AuthCallback(continueWith));
		}

		public async Task ResetPassword(string email, Action<bool> continueWith)
		{
			await FirebaseAuthService.Factory(FirebaseAccess.Instance).ResetPassword(GetCurrentUser().Email, continueWith);
		}
	}

	public class AuthCallback : IFirebaseAuthServiceCallback
	{
		private Action<bool> _continueWith;

		public AuthCallback(Action<bool> continueWith)
		{
			_continueWith = continueWith;
		}

		public void OnFail(Exception ex)
		{
			App.Current.MainPage.DisplayAlert("Error", "No se pudo completar la petición. Vuelve a intentar.", "ACEPTAR");
			if (_continueWith != null) _continueWith(false);
		}

		public async void OnSuccess(FirebaseAuthLink firebaseAuthLink)
		{
			if (firebaseAuthLink == null)
			{
				await App.Current.MainPage.DisplayAlert("Error", "Un error inesperado ocurrió.", "ACEPTAR");
				return;
			}
			var user = new Models.User();
			user.DisplayName = firebaseAuthLink.User.DisplayName;
			user.Email = firebaseAuthLink.User.Email;
			user.ObjectId = firebaseAuthLink.User.LocalId;
			user.FirstName = firebaseAuthLink.User.FirstName;
			user.LastName = firebaseAuthLink.User.LastName;
			user.ProfilePicture = firebaseAuthLink.User.PhotoUrl;
			UserManager.Instance.CurrentUser = user;
			Settings.FirebaseUserToken = firebaseAuthLink.FirebaseToken;
			await UserService.Instance.Register(user);

			if (_continueWith != null) _continueWith(true);
		}
	}

}

