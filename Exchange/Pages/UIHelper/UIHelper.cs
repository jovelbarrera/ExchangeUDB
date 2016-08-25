using System;
using Exchange.Configs;
using Xamarin.Forms;

namespace Exchange.UIHelper
{
	public class UIHelper
	{
		public static BoxView Separator()
		{
			return new BoxView
			{
				BackgroundColor = Styles.Colors.Placeholder,
				HeightRequest = 1,
				VerticalOptions = LayoutOptions.End
			};
		}

		public static StackLayout FormGroup(string imageSource, View view)
		{
			var layout = new StackLayout
			{
				Spacing = 10,
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
				Children = { UIHelper.Separator() }
			});
			return layout;
		}
	}
}

