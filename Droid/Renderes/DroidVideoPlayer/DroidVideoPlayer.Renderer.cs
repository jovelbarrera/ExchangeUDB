using System.ComponentModel;
using Android.Graphics;
using Exchange.Controls.VideoPlayer;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

namespace Exchange.Droid.DroidVideoPlayer
{
	public partial class DroidVideoPlayer : ViewRenderer,
                                          	Android.Views.ISurfaceHolderCallback
	{
		protected override void OnElementChanged(ElementChangedEventArgs<View> e)
		{
			base.OnElementChanged(e);

			if (e.OldElement != null || Element == null)
				return;

			var videoPlayer = e.NewElement as VideoPlayer;
			Asset = videoPlayer.Asset;
			Listener = videoPlayer.Listener;


			InitPlayer();
			AddView(_mainView);
		}

		protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			base.OnElementPropertyChanged(sender, e);
			if (e.PropertyName == VideoPlayer.ReleaseProperty.PropertyName)
				OnDestroy();
		}

		protected override void OnLayout(bool changed, int l, int t, int r, int b)
		{
			base.OnLayout(changed, l, t, r, b);

			var msw = MeasureSpec.MakeMeasureSpec(r - l, Android.Views.MeasureSpecMode.Exactly);
			var msh = MeasureSpec.MakeMeasureSpec(b - t, Android.Views.MeasureSpecMode.Exactly);

			_mainView.Measure(msw, msh);
			_mainView.Layout(0, 0, r - l, b - t);
		}

		#region SurfaceHolder.Callback implementation

		public void SurfaceCreated(Android.Views.ISurfaceHolder holder)
		{
			if (_player != null)
			{
				_player.Surface = holder.Surface;
			}
		}

		public void SurfaceChanged(Android.Views.ISurfaceHolder holder, Format format, int width, int height)
		{
			// Do nothing.
		}

		public void SurfaceDestroyed(Android.Views.ISurfaceHolder holder)
		{
			if (_player != null)
			{
				_player.BlockingClearSurface();
			}
		}

		#endregion
	}
}