using System;
using System.Linq;
using Exchange.Configs;
using Exchange.Exceptions;
using Exchange.Models;
using Exchange.Pages.Base;
using Exchange.Services;
using Exchange.Services.FirebaseServices;

namespace Exchange.Pages
{
	public partial class SignupPage : BasePage
	{
		public SignupPage()
		{
			InitializeComponents();

			//_firstnameEntry.Text = "Roberto Ernesto";
			//_lastnameEntry.Text = "Jovel Barrea";
			//_emailEntry.Text = "rej.barrera@gmail.com";
			//_passwordEntry.Text = "jovelbarrera";
			//_confirmPasswordEntry.Text = "jovelbarrera";
		}

		private async void RegisterButton_Clicked(object sender, EventArgs e)
		{
			DisableControls();
			try
			{
				if (!IsValid())
				{
					EnaleControls();
					return;
				}
				string email, password, firstname, lastname;
				firstname = _firstnameEntry.Text.Trim();
				lastname = _lastnameEntry.Text.Trim();
				email = _emailEntry.Text.Trim();
				password = _passwordEntry.Text;
				await StartAsyncTask(FirebaseAuthProvider.Instance.SignUp(email, password));
				PersistentUser user = await CustomUserManager.Instance.GetCurrentUser();
				user.DisplayName = firstname.Split(' ').FirstOrDefault();
				user.FirstName = firstname;
				user.LastName = lastname;
				await CustomUserManager.Instance.UpdateCurrentUser(user);
				App.Current.MainPage = new MainPage();
			}
			catch (Exception ex)
			{
				if (ex is NoInternetException)
					await DisplayAlert("Sin conexión", "No se pudo conectar a internet.", "ACEPTAR");
				else if (ex.Message == "400 (Bad Request)")
					await DisplayAlert("Error", "Ya hay un usuario registrado con este correo electrónico.", "ACEPTAR");
				else
					await DisplayAlert("Error", "Ocurrió un error al intentar crear la cuenta, intente de nuevo", "ACEPTAR");
			}
			finally
			{
				Content = MainLayout;
			}
			EnaleControls();
		}

		private bool IsValid()
		{
			if (string.IsNullOrEmpty(_firstnameEntry.Text))
				DisplayAlert("Datos no válidos", "Proporciona tu nombre", "ACEPTAR");
			if (string.IsNullOrEmpty(_lastnameEntry.Text))
				DisplayAlert("Datos no válidos", "Proporciona tu apellido", "ACEPTAR");
			if (string.IsNullOrEmpty(_emailEntry.Text) || !_emailEntry.Text.IsEmail())
				DisplayAlert("Datos no válidos", "Proporciona una cuenta de correo electrónico válida", "ACEPTAR");
			else if (string.IsNullOrEmpty(_passwordEntry.Text))
				DisplayAlert("Datos no válidos", "Escoje una contraseña para tu cuenta", "ACEPTAR");
			else if (_passwordEntry.Text != _confirmPasswordEntry.Text)
				DisplayAlert("Datos no válidos", "Las contraseñas no coinciden, por favor revisa nuevamente", "ACEPTAR");
			else if (_passwordEntry.Text.Length < 8)
				DisplayAlert("Datos no válidos", "Tu contraseña debe tener al menos 8 caracteres", "ACEPTAR");
			else
				return true;
			return false;
		}

		private void EnaleControls()
		{
			_firstnameEntry.IsEnabled = true;
			_lastnameEntry.IsEnabled = true;
			_emailEntry.IsEnabled = true;
			_passwordEntry.IsEnabled = true;
			_confirmPasswordEntry.IsEnabled = true;
			_registerButton.IsEnabled = true;
		}

		private void DisableControls()
		{
			_firstnameEntry.IsEnabled = false;
			_lastnameEntry.IsEnabled = false;
			_emailEntry.IsEnabled = false;
			_passwordEntry.IsEnabled = false;
			_confirmPasswordEntry.IsEnabled = false;
			_registerButton.IsEnabled = false;
		}
	}
}


