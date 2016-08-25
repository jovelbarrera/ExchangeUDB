using System;
using Exchange.Configs;
using Xamarin.Forms;

namespace Exchange.ContentViews
{
	public class ConnectionFailContent : ContentView
	{
		private EventHandler _onRetry;
		public ConnectionFailContent(EventHandler onRetry)
		{
			_onRetry = onRetry;

			var mainLayout = new StackLayout();
			BackgroundColor = Color.White;

			var notfoundLabel = new Label
			{
				Style = Styles.Subtitle,
				Text = "No se pudo conectar al servidor.\nPor favor verifica tu conección de internet",
				HorizontalTextAlignment = TextAlignment.Center,
				VerticalOptions = LayoutOptions.CenterAndExpand,
			};
			mainLayout.Children.Add(notfoundLabel);

			var retryButton = new Button
			{
				Style = Styles.ActiveButton,
				Text = "Intentar de Nuevo",
				VerticalOptions = LayoutOptions.Start,
			};
			if (_onRetry != null)
				retryButton.Clicked += _onRetry;
			mainLayout.Children.Add(retryButton);

			Content = mainLayout;
		}
	}
}


