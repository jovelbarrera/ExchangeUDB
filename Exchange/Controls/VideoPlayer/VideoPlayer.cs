using System;
using Xamarin.Forms;

namespace Exchange.Controls.VideoPlayer
{
	public class VideoPlayer : View
	{
		public static readonly BindableProperty AssetProperty =
				BindableProperty.Create("Asset", typeof(IVideoPlayerAsset), typeof(VideoPlayer), null);

		public IVideoPlayerAsset Asset
		{
			get { return (IVideoPlayerAsset)GetValue(AssetProperty); }
			set { SetValue(AssetProperty, value); }
		}

		public static readonly BindableProperty ListenerProperty =
			BindableProperty.Create("Listener", typeof(IVideoPlayerListener), typeof(VideoPlayer), null);

		public IVideoPlayerListener Listener
		{
			get { return (IVideoPlayerListener)GetValue(ListenerProperty); }
			set { SetValue(ListenerProperty, value); }
		}

		public static readonly BindableProperty ReleaseProperty =
			BindableProperty.Create("Release", typeof(bool), typeof(VideoPlayer), false);

		public bool Release
		{
			get { return (bool)GetValue(ReleaseProperty); }
			set { SetValue(ReleaseProperty, value); }
		}

		public VideoPlayer(IVideoPlayerAsset asset, IVideoPlayerListener listener = null)
		{
			Asset = asset;
			Listener = listener;
		}
	}
}