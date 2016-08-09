using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Exchange.Dependencies;
using Exchange.Dependencies.Facebook;
using Exchange.Models;
using Exchange.Services;
using Exchange.Services.FirebaseServices;
using Xamarin.Forms;

namespace Exchange.Pages
{
	public partial class LoginPage : ContentPage
	{
		public LoginPage()
		{
			InitializeComponents();
		}

		private void FbButton_Clicked(object sender, EventArgs e)
		{
			//((Button)sender).IsEnabled = false;
			try
			{
				var facebook = DependencyService.Get<IFacebookButton>();
				facebook.LoginWithReadPermissions(new[] { "email" }, ReadPermissionsCallback);
			}
			catch (Exception ex)
			{
				System.Diagnostics.Debug.WriteLine(ex.Message);
			}
		}

		private async void ReadPermissionsCallback(FacebookEvent e)
		{
			if (e == null)
			{
				await DisplayAlert("Error", "No se pudo iniciar sesión con facebook, intente de nuevo", "OK");
			}
			else
			{
				if (e.GrantedPermissions.Any(i => i == "email"))
					await UserManager.Instance.SignInWithFacebook(e.AccessToken, FacebookLoginCallback);
				else
					await DisplayAlert("Error", "Tu correo electrónico es necesario para completar el login con facebook, por favor acepta el permiso de correo electrónico", "OK");
			}

		}

		private void FacebookLoginCallback(bool isSuccessful)
		{
			if (isSuccessful)
				App.Current.MainPage = new MainPage();
			else
				DisplayAlert("Error", "No se pudo iniciar sesión con facebook, intente de nuevo", "OK");

		}
	}
}


