using System;
using System.ComponentModel;
using System.IO;
using Android.App;
using Android.Views;
using Android.Widget;
using com.refractored.fab;
using Exchange.Controls;
using Exchange.Droid.DroidFab;
using Xamarin.Forms.Platform.Android;
using XF = Xamarin.Forms;

[assembly: XF.ExportRenderer(typeof(Fab), typeof(DroidFab))]
namespace Exchange.Droid.DroidFab
{
	public class DroidFab : ViewRenderer//<Fab, FrameLayout>
	{
		private Activity _activity;
		private FrameLayout _view;

		protected override void OnElementChanged(ElementChangedEventArgs<XF.View> e)
		{
			base.OnElementChanged(e);

			if (e.OldElement != null || Element == null)
				return;

			Fab fab = ((Fab)Element);
			Activity activity = Context as Activity;
			_view = (FrameLayout)activity.LayoutInflater.Inflate(Resource.Layout.Fab, this, false);
			var f = _view.GetChildAt(0);
			//var f = FindViewById(Resource.Id.fab);
			f.Click += (sender, arg) =>
			{
				fab.Click.Invoke(sender, arg);
			};
			AddView(_view);
		}

		protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			base.OnElementPropertyChanged(sender, e);
			if (Element == null)
				return;

			//Fab fab = ((Fab)Element);
			//if (e.PropertyName == Fab.ClickProperty.PropertyName)
			//{
			//	if (_view != null && fab.Click != null)
			//		_view.Click += fab.Click;
			//}
		}

		protected override void OnLayout(bool changed, int l, int t, int r, int b)
		{
			base.OnLayout(changed, l, t, r, b);

			var msw = MeasureSpec.MakeMeasureSpec(r - l, MeasureSpecMode.Exactly);
			var msh = MeasureSpec.MakeMeasureSpec(b - t, MeasureSpecMode.Exactly);

			_view.Measure(msw, msh);
			_view.Layout(0, 0, r - l, b - t);
		}
	}
}