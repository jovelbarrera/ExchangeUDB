using System;
using Exchange.Configs;
using Xamarin.Forms;

namespace Exchange.UIHelper
{
	public class UIHelper
	{
		//public NavigationPage Navigation()
		//{
		//	NavigationPage
		//}

		public static BoxView Separator()
		{
			return Separator(Styles.Colors.Placeholder);
		}

		public static BoxView Separator(Color color)
		{
			return new BoxView
			{
				BackgroundColor = color,
				HeightRequest = 1,
				VerticalOptions = LayoutOptions.End
			};
		}

		public static StackLayout FormGroup(string imageSource, View view)
		{
			return FormGroup(imageSource, view, Styles.Colors.Placeholder);
		}

		public static StackLayout FormGroup(string imageSource, View view, Color baseLineColor)
		{
			var layout = new StackLayout
			{
				Spacing = 0,
			};
			var goupLayout = new StackLayout
			{
				Orientation = StackOrientation.Horizontal,
				Children =
				{
					new Image
					{
						Aspect=Aspect.Fill,
						VerticalOptions=LayoutOptions.Center,
						HeightRequest=25,
						WidthRequest=25,
						Source=imageSource,
					},
					view,
				}
			};
			layout.Children.Add(goupLayout);
			layout.Children.Add(new StackLayout
			{
				Padding = new Thickness(30, 0, 0, 0),
				Children = { UIHelper.Separator(baseLineColor) }
			});
			return layout;
		}
	}
}

