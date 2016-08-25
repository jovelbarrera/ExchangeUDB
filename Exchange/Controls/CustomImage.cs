using System;
using Xamarin.Forms;

namespace Exchange.Controls
{
	public class CustomImage : Image
	{
		public static readonly BindableProperty BorderWidthProperty =
			BindableProperty.Create<CustomImage, float>(
				p => p.BorderWidth, 1
			);

		public float BorderWidth
		{
			get { return (float)GetValue(BorderWidthProperty); }
			set { SetValue(BorderWidthProperty, value); }
		}

		public static readonly BindableProperty BorderColorProperty =
			BindableProperty.Create<CustomImage, Color>(
				p => p.BorderColor, Color.Transparent
			);

		public Color BorderColor
		{
			get { return (Color)GetValue(BorderColorProperty); }
			set { SetValue(BorderColorProperty, value); }
		}
	}
}

