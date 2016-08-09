using System.ComponentModel;
using Exchange.Droid.Renderers;
using Exchange.Controls;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using System;
using Android.Graphics;
using Android.Content;
using Android.Graphics.Drawables;
using Android.Content.Res;
using System.Collections.Generic;

[assembly: ExportRenderer(
	typeof(CustomEditor),
	typeof(DroidCustomEditor)
)]
namespace Exchange.Droid.Renderers
{
	public class DroidCustomEditor : EditorRenderer
	{
		protected Xamarin.Forms.Color BackgroundColor
		{
			get;
			private set;
		}
		protected override void OnElementChanged(ElementChangedEventArgs<Editor> e)
		{
			base.OnElementChanged(e);
			if (e.OldElement == null || Element == null)
				return;

			var customEditor = (CustomEditor)Element;
			SetControlBackground();
			Control.SetBackgroundColor(Element.BackgroundColor.ToAndroid());
			Control.SetPadding(10, 0, 10, 0);
			UpdateLayout();
		}

		protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			base.OnElementPropertyChanged(sender, e);
			if (this == null)
				return;
			var customEditor = (CustomEditor)Element;
			SetControlBackground();
			Control.SetBackgroundColor(Element.BackgroundColor.ToAndroid());
			Control.SetPadding(10, 0, 10, 0);
			UpdateLayout();
		}

		private void SetControlBackground()
		{
			if (Element.BackgroundColor != Xamarin.Forms.Color.Transparent)
			{
				BackgroundColor = Element.BackgroundColor;
			}
			Element.BackgroundColor = Xamarin.Forms.Color.Transparent;
		}

		protected override bool DrawChild(
			Android.Graphics.Canvas canvas,
			Android.Views.View child,
			long drawingTime
		)
		{
			var editor = Element as CustomEditor;
			var rect = new Rect();
			var paint = new Paint()
			{
				Color = BackgroundColor.ToAndroid(),
				AntiAlias = true,
			};

			GetDrawingRect(rect);

			var radius = (float)(rect.Width() / editor.Width * 2/*editor.CornerRadius*/);
			canvas.DrawRoundRect(new RectF(rect), radius, radius, paint);
			canvas.Save();
			var result = base.DrawChild(canvas, child, drawingTime);

			paint.Dispose();
			return result;
		}
	}
}


