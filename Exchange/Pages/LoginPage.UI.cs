using System;
using Xamarin.Forms;

namespace Exchange.Pages
{
	public partial class LoginPage
	{
		private void InitializeComponents()
		{
			BackgroundColor = Color.White;

			var mainLayout = new StackLayout
			{
				Padding = 20,
				VerticalOptions = LayoutOptions.CenterAndExpand
			};

			var exchangeLogo = new Image
			{
				//WidthRequest = 120,
				Source = "ic_exchange_logo.png",
			};
			mainLayout.Children.Add(exchangeLogo);

			var fbButton = new Button
			{
				TextColor = Color.White,
				BackgroundColor = Configs.Styles.Colors.FB,
				Text = "INICIAR SESIÓN",
				WidthRequest = 300,
				Image = "ic_facebook.png",
				HorizontalOptions = LayoutOptions.CenterAndExpand
			};
			fbButton.Clicked+= FbButton_Clicked;
			mainLayout.Children.Add(fbButton);

			Content = mainLayout;
		}
	}
}


