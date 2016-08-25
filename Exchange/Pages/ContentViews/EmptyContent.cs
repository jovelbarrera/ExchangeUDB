using System;
using Exchange.Configs;
using Xamarin.Forms;

namespace Exchange.ContentViews
{
	public class EmptyContent : ContentView
	{
		public EmptyContent()
		{
			var mainLayout = new StackLayout();
			BackgroundColor = Color.White;

			var notfoundLabel = new Label
			{
				Style = Styles.Subtitle,
				Text = "Sin nada que mostrar",
				HorizontalTextAlignment = TextAlignment.Center,
				VerticalOptions = LayoutOptions.CenterAndExpand,
			};
			mainLayout.Children.Add(notfoundLabel);

			Content = mainLayout;
		}
	}
}


