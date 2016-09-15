using System;
using Exchange.Configs;
using Exchange.Controls;
using Xamarin.Forms;

namespace Exchange.Pages
{
	public partial class EmailLoginPage
	{
		private CustomEntry _emailEntry;
		private Label _noteLabel;
		private CustomEntry _passwordEntry;
		private Button _loginButton;
		private Button _resetPasswordButton;
		private Label _forgotPasswordLabel;
		private StackLayout _passwordLayout;
		private StackLayout _bottomLayout;

		protected override void InitializeComponents()
		{
			BackgroundColor = Color.White;

			var contentLayout = new StackLayout
			{
				Spacing = 20,
				VerticalOptions = LayoutOptions.FillAndExpand
			};

			var centerLayout = new StackLayout
			{
				Padding = new Thickness(40, 20),
				VerticalOptions = LayoutOptions.CenterAndExpand
			};
			contentLayout.Children.Add(centerLayout);

			var exchangeLogo = new Image
			{
				//WidthRequest = 120,
				Source = "ic_exchange_logo.png",
			};
			centerLayout.Children.Add(exchangeLogo);

			_emailEntry = new CustomEntry
			{
				Style = Styles.NormalEntry,
				Placeholder = "Email",
				Keyboard = Keyboard.Email,
			};
			centerLayout.Children.Add(UIHelper.UIHelper.FormGroup("ic_mail.png", _emailEntry, Color.Transparent));
			centerLayout.Children.Add(UIHelper.UIHelper.Separator());
			_noteLabel = new Label
			{
				Style = Styles.Verbosa,
				FontSize = Device.GetNamedSize(NamedSize.Micro, typeof(Label)),
				IsVisible = false,
				Text = "Proporciona el correo viculado a tu cuenta para reestablecer tu contraseña",
			};
			centerLayout.Children.Add(_noteLabel);
			_passwordEntry = new CustomEntry
			{
				IsPassword = true,
				Style = Styles.NormalEntry,
				Placeholder = "Contraseña",
			};
			_passwordLayout = UIHelper.UIHelper.FormGroup("ic_password.png", _passwordEntry, Color.Transparent);
			centerLayout.Children.Add(_passwordLayout);

			centerLayout.Children.Add(new BoxView
			{
				BackgroundColor = Color.Transparent,
				HeightRequest = 60,
			});

			_loginButton = new Button
			{
				Style = Styles.ActiveButton,
				Text = "INICIAR SESIÓN",
				HeightRequest = 60,
				//Image = "ic_facebook.png",
				HorizontalOptions = LayoutOptions.FillAndExpand
			};
			_loginButton.Clicked += LoginButton_Clicked;
			centerLayout.Children.Add(_loginButton);

			_resetPasswordButton = new Button
			{
				Style = Styles.ActiveButton,
				Text = "REESTABLECER CONTRASEÑA",
				HeightRequest = 60,
				HorizontalOptions = LayoutOptions.FillAndExpand,
				IsVisible = false,
			};
			_resetPasswordButton.Clicked += ResetPasswordButton_Clicked;
			centerLayout.Children.Add(_resetPasswordButton);

			_bottomLayout = new StackLayout
			{
				BackgroundColor = Styles.Colors.Placeholder,
				VerticalOptions = LayoutOptions.End
			};
			contentLayout.Children.Add(_bottomLayout);

			_forgotPasswordLabel = new Label
			{
				Style = Styles.Verbosa,
				TextColor = Styles.Colors.Primary,
				HorizontalTextAlignment = TextAlignment.Center,
				VerticalOptions = LayoutOptions.End,
				FontAttributes = FontAttributes.Bold,
				HeightRequest = 60,
				Text = "Olvidé mi clave",
			};
			_forgotPasswordLabel.AddTapHandler(ForgotPasswordButton_Clicked);
			_bottomLayout.Children.Add(_forgotPasswordLabel);

			MainLayout = new ScrollView
			{
				Content = contentLayout,
			};
		}
	}
}


