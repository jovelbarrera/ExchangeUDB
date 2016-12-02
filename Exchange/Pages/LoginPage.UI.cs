using System;
using Exchange.Configs;
using Exchange.Controls;
using Xamarin.Forms;

namespace Exchange.Pages
{
	public partial class LoginPage
	{
		private Button _loginButton;
		private Button _fbButton;
		private Label _registerLabel;

		protected override void InitializeComponents()
		{
			NavigationPage.SetHasNavigationBar(this, false);
			BackgroundColor = Color.White;

			var contentLayout = new StackLayout
			{
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

			_fbButton = new Button
			{
				TextColor = Color.White,
				BackgroundColor = Styles.Colors.FB,
				Text = "INICIAR SESIÓN CON FACEBOOK",
				Image = "ic_facebook.png",
				HorizontalOptions = LayoutOptions.FillAndExpand,
			};
			_fbButton.Clicked += FbButton_Clicked;
			centerLayout.Children.Add(_fbButton);

			_loginButton = new Button
			{
				Style = Styles.ActiveButton,
				Text = "INICIAR SESIÓN CON EMAIL",
				Image = "ic_email_white.png",
				HorizontalOptions = LayoutOptions.FillAndExpand
			};
			_loginButton.Clicked += LoginButton_Clicked;
			centerLayout.Children.Add(_loginButton);

			var bottomLayout = new StackLayout
			{
				BackgroundColor = Styles.Colors.Placeholder,
				VerticalOptions = LayoutOptions.End
			};
			contentLayout.Children.Add(bottomLayout);

			_registerLabel = new Label
			{
				Style = Styles.Verbosa,
				TextColor = Styles.Colors.Primary,
				HorizontalTextAlignment = TextAlignment.Center,
				VerticalOptions = LayoutOptions.End,
				HeightRequest = 60,
				//Text = "Crear una cuenta",
			};
			var span1 = new Span
			{
				Text = "¿No estás registrado aún? ",
			};
			var span2 = new Span
			{
				FontAttributes = FontAttributes.Bold,
				Text = "Crea una cuenta",
			};
			_registerLabel.FormattedText = new FormattedString
			{
				Spans = { span1, span2 }
			};
			_registerLabel.AddTapHandler(RegisterButton_Clicked);
			bottomLayout.Children.Add(_registerLabel);

			MainLayout = new ScrollView
			{
				Content = contentLayout,
			};
			//Content = mainLayout;
		}
	}
}


