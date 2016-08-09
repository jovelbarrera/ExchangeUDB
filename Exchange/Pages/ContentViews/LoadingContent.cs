using System;
using Exchange.Configs;
using Xamarin.Forms;

namespace Exchange.ContentViews
{
	public class LoadingContent : ContentView
	{
		public LoadingContent()
		{
			var mainLayout = new StackLayout();
			BackgroundColor = Color.White;

			var activityIndicator = new ActivityIndicator
			{
				Color = Styles.Colors.Primary,
				VerticalOptions = LayoutOptions.CenterAndExpand,
				IsRunning = true,
			};
			mainLayout.Children.Add(activityIndicator);

			Content = mainLayout;
		}
	}
}


