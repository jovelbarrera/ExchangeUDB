using System;
using Firebase.Xamarin.Auth;

namespace Exchange.Services.FirebaseServices
{
	public interface IFirebaseAuthServiceCallback
	{
		void OnSuccess(FirebaseAuthLink firebaseAuthLink);
		void OnFail(Exception ex);
	}
}
