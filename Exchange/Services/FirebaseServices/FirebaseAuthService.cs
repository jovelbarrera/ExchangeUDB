using System;
using System.Threading.Tasks;
using Exchange.Configs;
using Firebase.Xamarin.Auth;

namespace Exchange.Services.FirebaseServices
{
	public class FirebaseAuthService
	{
		private FirebaseAuthProvider authProvider;

		public FirebaseAuthService(IFirebaseAccess firebaseAccess)
		{
			authProvider = new FirebaseAuthProvider(new FirebaseConfig(firebaseAccess.ApiKey));
		}

		public static FirebaseAuthService Factory(IFirebaseAccess firebaseAccess)
		{
			return new FirebaseAuthService(firebaseAccess);
		}

		public async Task SignUpWithCredentials(string email, string password, IFirebaseAuthServiceCallback callback)
		{
			try
			{
				FirebaseAuthLink auth = await authProvider.CreateUserWithEmailAndPasswordAsync(email, password);
				if (callback != null) callback.OnSuccess(auth);
			}
			catch (Exception ex)
			{
				if (callback != null) callback.OnFail(ex);
			}
		}

		public async Task SignInWithFacebook(string facebookAccessToken, IFirebaseAuthServiceCallback callback)
		{
			try
			{
				FirebaseAuthLink auth = await authProvider.SignInWithOAuthAsync(FirebaseAuthType.Facebook, facebookAccessToken);
				if (callback != null) callback.OnSuccess(auth);
			}
			catch (Exception ex)
			{
				if (callback != null) callback.OnFail(ex);
			}
		}

		public async Task SignInWithCredentials(string email, string password, IFirebaseAuthServiceCallback callback)
		{
			try
			{
				FirebaseAuthLink auth = await authProvider.SignInWithEmailAndPasswordAsync(email, password);
				if (callback != null) callback.OnSuccess(auth);
			}
			catch (Exception ex)
			{
				if (callback != null) callback.OnFail(ex);
			}
		}

		public async Task ResetPassword(string email, Action<bool> callback)
		{
			try
			{
				await authProvider.SendPasswordResetEmailAsync(email);
				if (callback != null) callback(true);
			}
			catch (Exception ex)
			{
				if (callback != null) callback(false);
			}
		}
	}
}
