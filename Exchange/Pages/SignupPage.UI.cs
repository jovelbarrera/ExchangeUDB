using System;
using Exchange.Configs;
using Exchange.Controls;
using Xamarin.Forms;

namespace Exchange.Pages
{
	public partial class SignupPage
	{
		private CustomEntry _firstnameEntry;
		private CustomEntry _lastnameEntry;
		private CustomEntry _emailEntry;
		private CustomEntry _passwordEntry;
		private CustomEntry _confirmPasswordEntry;
		private Button _registerButton;

		protected override void InitializeComponents()
		{
			BackgroundColor = Color.White;

			var contentLayout = new StackLayout
			{
				Spacing = 10,
				Padding = new Thickness(40, 20),
				VerticalOptions = LayoutOptions.CenterAndExpand
			};

			var exchangeLogo = new Image
			{
				Source = "ic_exchange_logo.png",
			};
			contentLayout.Children.Add(exchangeLogo);

			_firstnameEntry = new CustomEntry
			{
				Style = Styles.NormalEntry,
				Placeholder = "Nombres",
			};
			contentLayout.Children.Add(UIHelper.UIHelper.FormGroup("ic_user.png", _firstnameEntry));
			_lastnameEntry = new CustomEntry
			{
				Style = Styles.NormalEntry,
				Placeholder = "Apellidos",
			};
			contentLayout.Children.Add(UIHelper.UIHelper.FormGroup(null, _lastnameEntry));
			//mainLayout.Children.Add(UIHelper.UIHelper.Separator());
			_emailEntry = new CustomEntry
			{
				Style = Styles.NormalEntry,
				Placeholder = "Email",
				Keyboard = Keyboard.Email,
			};
			contentLayout.Children.Add(UIHelper.UIHelper.FormGroup("ic_mail.png", _emailEntry));
			////mainLayout.Children.Add(UIHelper.UIHelper.Separator());
			_passwordEntry = new CustomEntry
			{
				IsPassword = true,
				Style = Styles.NormalEntry,
				Placeholder = "Contraseña",
			};
			contentLayout.Children.Add(UIHelper.UIHelper.FormGroup("ic_password.png", _passwordEntry));
			//mainLayout.Children.Add(UIHelper.UIHelper.Separator());
			_confirmPasswordEntry = new CustomEntry
			{
				IsPassword = true,
				Style = Styles.NormalEntry,
				Placeholder = "Confirmar Contraseña",
			};
			contentLayout.Children.Add(UIHelper.UIHelper.FormGroup("ic_password.png", _confirmPasswordEntry));

			contentLayout.Children.Add(new BoxView
			{
				BackgroundColor = Color.Transparent,
				HeightRequest = 60,
			});

			_registerButton = new Button
			{
				Style = Styles.ActiveButton,
				Text = "CREAR CUENTA",
				HeightRequest = 60,
				HorizontalOptions = LayoutOptions.FillAndExpand,
				VerticalOptions = LayoutOptions.End,
			};
			_registerButton.Clicked += RegisterButton_Clicked;
			contentLayout.Children.Add(_registerButton);

			MainLayout = new ScrollView
			{
				Content = contentLayout,
			};
		}
	}
}


