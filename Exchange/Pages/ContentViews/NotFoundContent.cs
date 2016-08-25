using System;
using Exchange.Configs;
using Xamarin.Forms;

namespace Exchange.ContentViews
{
	public class NotFoundContent : ContentView
	{
		public NotFoundContent()
		{
			var mainLayout = new StackLayout();
			BackgroundColor = Color.White;

			var notfoundLabel = new Label
			{
				Style = Styles.Subtitle,
				Text = "Ups... El contenido no fue hayado",
				HorizontalTextAlignment = TextAlignment.Center,
				VerticalOptions = LayoutOptions.CenterAndExpand,
			};
			mainLayout.Children.Add(notfoundLabel);

			Content = mainLayout;
		}
	}
}


