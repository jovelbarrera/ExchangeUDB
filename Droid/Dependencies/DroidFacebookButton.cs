using System;
using System.Linq;
using Exchange.Dependencies.Facebook;
using Exchange.Droid;
using Java.Interop;
using Java.Util;
using Kadevjo.Droid.Dependencies;
using Xamarin.Facebook;
using Xamarin.Facebook.Login;
using Xamarin.Forms;

[assembly: Dependency(typeof(DroidFacebookButton))]
namespace Kadevjo.Droid.Dependencies
{
	public class DroidFacebookButton : IFacebookButton
	{
		private Action<FacebookEvent> _callback;

		public DroidFacebookButton()
		{
			MainActivity.CallbackManager = CallbackManagerFactory.Create();

			LoginManager.Instance.RegisterCallback(
				MainActivity.CallbackManager,
				new FacebookCallback<LoginResult>
				{
					HandleSuccess = (result) =>
					{
						Date expires = result.AccessToken.Expires;
						var epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
						DateTime expiration = epoch.AddMilliseconds(expires.Time);

						string[] grantedPermissions = default(string[]);
						if (AccessToken.CurrentAccessToken != null)
						{
							grantedPermissions = AccessToken.CurrentAccessToken.Permissions.ToArray();
						}

						if (_callback != null)
						{
							FacebookEvent data = new FacebookEvent()
							{
								AccessToken = result.AccessToken.Token,
								TokenExpiration = expiration,
								UserId = result.AccessToken.UserId,
								GrantedPermissions = grantedPermissions,
							};
							_callback(data);
						}
					},
					HandleCancel = () =>
					{
						if (_callback != null)
							_callback(default(FacebookEvent));

					},
					HandleError = (error) =>
					{
						if (_callback != null)
							_callback(default(FacebookEvent));

					}
				});
		}

		#region IFacebook implementation

		public void LoginWithReadPermissions(string[] permissions, Action<FacebookEvent> callback)
		{
			_callback = callback;
			LoginManager.Instance.SetLoginBehavior(LoginBehavior.NativeWithFallback);
			LoginManager.Instance.LogInWithReadPermissions(MainActivity.Instance, permissions);
		}

		public void LoginWithWritePermissions(string[] permissions, Action<FacebookEvent> callback)
		{
			_callback = callback;
			LoginManager.Instance.SetLoginBehavior(LoginBehavior.NativeWithFallback);
			LoginManager.Instance.LogInWithReadPermissions(MainActivity.Instance, permissions);
		}

		public void Logout()
		{
			LoginManager.Instance.LogOut();
		}

		#endregion
	}

	public class FacebookCallback<TResult> : Java.Lang.Object, IFacebookCallback where TResult : Java.Lang.Object
	{
		public Action HandleCancel { get; set; }
		public Action<FacebookException> HandleError { get; set; }
		public Action<TResult> HandleSuccess { get; set; }

		public void OnCancel()
		{
			var handler = HandleCancel;
			if (handler != null)
				handler();
		}

		public void OnError(FacebookException error)
		{
			var handler = HandleError;
			if (handler != null)
				handler(error);
		}

		public void OnSuccess(Java.Lang.Object result)
		{
			var handler = HandleSuccess;
			if (handler != null)
				handler(result.JavaCast<TResult>());
		}
	}
}