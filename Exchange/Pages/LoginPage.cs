using System;
using System.Linq;
using Exchange.Dependencies.Facebook;
using Exchange.Exceptions;
using Exchange.Pages.Base;
using Exchange.Services.FirebaseServices;
using Xamarin.Forms;

namespace Exchange.Pages
{
	public partial class LoginPage : BasePage
	{
		private void LoginButton_Clicked(object sender, EventArgs e)
		{
			DisableControls();
			Navigation.PushModalAsync(new EmailLoginPage());
			EnaleControls();
		}

		private void RegisterButton_Clicked(object sender, EventArgs e)
		{
			DisableControls();
			Navigation.PushModalAsync(new SignupPage());
			EnaleControls();
		}

		private void FbButton_Clicked(object sender, EventArgs e)
		{
			DisableControls();
			var facebook = DependencyService.Get<IFacebookButton>();
			facebook.LoginWithReadPermissions(new[] { "email" }, ReadPermissionsCallback);
		}

		private async void ReadPermissionsCallback(FacebookToken facebookToken)
		{
			if (facebookToken == null)
			{
				await DisplayAlert("Error", "No se pudo iniciar sesión con facebook, intente de nuevo", "ACEPTAR");
			}
			else
			{
				if (facebookToken.GrantedPermissions.Any(i => i == "email"))
				{
					try
					{
						await StartAsyncTask(FirebaseAuthProvider.Instance.LogIn(facebookToken));
						App.Current.MainPage = new MainPage();
					}
					catch (Exception ex)
					{
						if (ex is NoInternetException)
							await DisplayAlert("Sin conexión", "No se pudo conectar a internet.", "ACEPTAR");
						else
							await DisplayAlert("Error", "No se pudo iniciar sesión con facebook, intente de nuevo", "ACEPTAR");
					}
				}
				else
				{
					await DisplayAlert("Error", "Tu correo electrónico es necesario para completar el login con facebook, por favor acepta el permiso de correo electrónico", "ACEPTAR");
				}
			}
			EnaleControls();
		}


		private void EnaleControls()
		{
			_fbButton.IsEnabled = true;
			_loginButton.IsEnabled = true;
		}

		private void DisableControls()
		{
			_fbButton.IsEnabled = false;
			_loginButton.IsEnabled = false;
		}
	}
}


