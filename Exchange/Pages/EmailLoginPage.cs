using System;
using Exchange.Configs;
using Exchange.Exceptions;
using Exchange.Pages.Base;
using Exchange.Services.FirebaseServices;

namespace Exchange.Pages
{
	public partial class EmailLoginPage : BasePage
	{
		private async void LoginButton_Clicked(object sender, EventArgs e)
		{
			DisableControls();
			try
			{
				if (!IsValid())
				{
					EnaleControls();
					return;
				}
				string email, password;
				email = _emailEntry.Text.Trim();
				password = _passwordEntry.Text;
				await StartAsyncTask(FirebaseAuthProvider.Instance.LogIn(email, password));
				App.Current.MainPage = new MainPage();
			}
			catch (Exception ex)
			{
				if (ex is NoInternetException)
					await DisplayAlert("Sin conexión", "No se pudo conectar a internet.", "ACEPTAR");
				else
					await DisplayAlert("Error", "Tu correo y contraseña no coinciden.", "ACEPTAR");
			}
			finally
			{
				Content = MainLayout;
			}
			EnaleControls();
		}

		private void ForgotPasswordButton_Clicked(object sender, EventArgs e)
		{
			_noteLabel.IsVisible = true;
			_resetPasswordButton.IsVisible = true;
			_loginButton.IsVisible = false;
			_passwordLayout.IsVisible = false;
			_bottomLayout.IsVisible = false;
		}

		private async void ResetPasswordButton_Clicked(object sender, EventArgs e)
		{
			DisableControls();
			try
			{
				if (string.IsNullOrEmpty(_emailEntry.Text) || !_emailEntry.Text.IsEmail())
				{
					await DisplayAlert("Datos no válidos", "Proporciona una cuenta de correo electrónico válida", "ACEPTAR");
					return;
				}
				string email = _emailEntry.Text;
				await StartAsyncTask(FirebaseAuthProvider.Instance.ResetPassword(email));
				await DisplayAlert("Correo Enviado", "Se ha enviado un correo a tu cuenta con los pasos para poder reestablecer tu contraseña.", "ACEPTAR");
				await Navigation.PopModalAsync(true);
			}
			catch (Exception ex)
			{
				if (ex is NoInternetException)
					await DisplayAlert("Sin conexión", "No se pudo conectar a internet.", "ACEPTAR");
				else
					await DisplayAlert("Error", "No se pudo completar esta acción.", "ACEPTAR");
			}
			finally
			{
				Content = MainLayout;
			}
			EnaleControls();
		}

		private bool IsValid()
		{
			if (string.IsNullOrEmpty(_emailEntry.Text) || !_emailEntry.Text.IsEmail())
				DisplayAlert("Datos no válidos", "Proporciona una cuenta de correo electrónico válida", "ACEPTAR");
			else if (string.IsNullOrEmpty(_passwordEntry.Text))
				DisplayAlert("Datos no válidos", "Proporciona tu contraseña", "ACEPTAR");
			else
				return true;
			return false;
		}

		private void EnaleControls()
		{
			_emailEntry.IsEnabled = true;
			_passwordEntry.IsEnabled = true;
			_loginButton.IsEnabled = true;
			_resetPasswordButton.IsEnabled = true;
		}

		private void DisableControls()
		{
			_emailEntry.IsEnabled = false;
			_passwordEntry.IsEnabled = false;
			_loginButton.IsEnabled = false;
			_resetPasswordButton.IsEnabled = false;
		}
	}
}


