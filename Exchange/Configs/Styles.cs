using System;
using Exchange.Controls;
using Xamarin.Forms;

namespace Exchange.Configs
{
	public class Styles
	{
		public class Colors
		{
			public static readonly Color Primary = Color.FromHex("#23a4b8");
			public static readonly Color Secondary = Color.FromHex("#ef5a87");
			public static readonly Color Third = Color.FromHex("#fbd136");
			public static readonly Color Title = Color.FromHex("#38a495");
			public static readonly Color Subtitle = Color.FromHex("#b1b1b1");
			public static readonly Color NormalText = Color.FromHex("#6d7172");
			public static readonly Color Placeholder = Color.FromHex("#eeeeee");
			public static readonly Color FB = Color.FromHex("#3b5998");
		}

		public static readonly Style Title = new Style(typeof(Label))
		{
			Setters = {
				new Setter { Property = Label.TextColorProperty, Value = Colors.Title },
				new Setter { Property = Label.FontAttributesProperty, Value = FontAttributes.Bold },
				new Setter { Property = Label.FontSizeProperty, Value = Device.GetNamedSize (NamedSize.Medium, typeof(Label)) },
				new Setter { Property = Label.VerticalTextAlignmentProperty, Value = TextAlignment.Center },
			}
		};

		public static readonly Style Subtitle = new Style(typeof(Label))
		{
			Setters = {
				new Setter { Property = Label.TextColorProperty, Value = Colors.Subtitle },
				new Setter { Property = Label.FontAttributesProperty, Value = FontAttributes.Bold },
				new Setter { Property = Label.FontSizeProperty, Value = Device.GetNamedSize (NamedSize.Small, typeof(Label)) },
				new Setter { Property = Label.VerticalTextAlignmentProperty, Value = TextAlignment.Center },
			}
		};

		public static readonly Style Link = new Style(typeof(Label))
		{
			Setters = {
				new Setter { Property = Label.TextColorProperty, Value = Colors.Title },
				new Setter { Property = Label.FontSizeProperty, Value = Device.GetNamedSize (NamedSize.Small, typeof(Label)) },
				new Setter { Property = Label.VerticalTextAlignmentProperty, Value = TextAlignment.Center },
			}
		};

		public static readonly Style Verbosa = new Style(typeof(Label))
		{
			Setters = {
				new Setter { Property = Label.TextColorProperty, Value = Colors.NormalText },
				new Setter { Property = Label.FontSizeProperty, Value = Device.GetNamedSize (NamedSize.Small, typeof(Label)) },
				new Setter { Property = Label.VerticalTextAlignmentProperty, Value = TextAlignment.Center },
			}
		};

		public static readonly Style NormalEntry = new Style(typeof(CustomEntry))
		{
			Setters = {
				new Setter { Property = CustomEntry.TextColorProperty, Value = Colors.NormalText },
				new Setter { Property = CustomEntry.PlaceholderColorProperty, Value = Colors.Subtitle },
				new Setter { Property = CustomEntry.BackgroundColorProperty, Value = Color.White },
				new Setter { Property = CustomEntry.FontSizeProperty, Value = Device.GetNamedSize (NamedSize.Small, typeof(Label)) },
				new Setter { Property = CustomEntry.HorizontalOptionsProperty, Value = LayoutOptions.FillAndExpand },
			}
		};

		public static readonly Style NormalEditor = new Style(typeof(CustomEditor))
		{
			Setters = {
				new Setter { Property = CustomEditor.TextColorProperty, Value = Colors.NormalText },
				new Setter { Property = CustomEditor.BackgroundColorProperty, Value = Color.White },
				new Setter { Property = CustomEditor.FontSizeProperty, Value = Device.GetNamedSize (NamedSize.Small, typeof(Label)) },
				new Setter { Property = CustomEditor.HorizontalOptionsProperty, Value = LayoutOptions.FillAndExpand },
			}
		};

		public static readonly Style ActiveButton = new Style(typeof(Button))
		{
			Setters = {
				new Setter { Property = Button.TextColorProperty, Value = Color.White },
				new Setter { Property = Button.BackgroundColorProperty, Value = Colors.Primary },
				new Setter { Property = Button.FontSizeProperty, Value = Device.GetNamedSize (NamedSize.Small, typeof(Label)) },
			}
		};

		public static readonly Style LikeButton = new Style(typeof(Button))
		{
			Setters = {
				new Setter { Property = Button.TextColorProperty, Value = Colors.NormalText },
				new Setter { Property = Button.BackgroundColorProperty, Value = Color.FromHex("#EFEFEF") },
				new Setter { Property = Button.FontSizeProperty, Value = Device.GetNamedSize (NamedSize.Small, typeof(Label)) },
				new Setter { Property = Button.FontAttributesProperty, Value = FontAttributes.Bold },
			}
		};
	}
}

