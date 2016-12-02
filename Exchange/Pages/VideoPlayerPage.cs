using System;
using System.IO;
using System.Reflection;
using Exchange.Controls.VideoPlayer;
using Exchange.Dependencies;
using Exchange.Pages.Base;
using Xamarin.Forms;

namespace Exchange.Pages
{
	public partial class VideoPlayerPage : BasePage
	{
		private bool _useFallback;
		private WebView _webPlayer;
		private VideoPlayer _nativePlayer;

		public IVideoPlayerAsset Asset { get; private set; }
		public IVideoPlayerListener Listener { get; private set; }

		public VideoPlayerPage(IVideoPlayerAsset asset, IVideoPlayerListener listener = null, bool useFallback = false)
		{
			IPlatform platform = DependencyService.Get<IPlatform>();
			platform.ChangeScreenOrientation(ScreenOrientation.Sensor);
			Asset = asset;
			Listener = listener;
			_useFallback = useFallback;
			Init(_useFallback);
		}

		private void Init(bool useFallback)
		{
			if (useFallback)
				RenderWebPlayer();
			else
				RenderNativePlayer();
		}

		private void RenderNativePlayer()
		{
			_nativePlayer = new VideoPlayer(Asset, Listener);
			Content = PlayerUI(_nativePlayer);
		}

		private void RenderWebPlayer()
		{
			// TODO test web player
			Stream stream = null;
			Device.OnPlatform(
				() => { stream = typeof(VideoPlayerPage).GetTypeInfo().Assembly.GetManifestResourceStream("iOSplayer.html"); },
				() => { stream = typeof(VideoPlayerPage).GetTypeInfo().Assembly.GetManifestResourceStream("androidPlayer.html"); });

			if (stream == null)
				return;

			string html = string.Empty;
			using (var reader = new StreamReader(stream))
			{
				html = reader.ReadToEnd();
			}
			html = html.Replace("{videoUri}", Asset.VideoUrl).Replace("{token}", Asset.Token);

			_webPlayer = new WebView()
			{
				Source = new HtmlWebViewSource { Html = html },
				VerticalOptions = LayoutOptions.FillAndExpand,
				HorizontalOptions = LayoutOptions.FillAndExpand,
			};
			Content = PlayerUI(_webPlayer);
		}

		protected override void OnDisappearing()
		{
			base.OnDisappearing();
			if (_webPlayer != null)
				_webPlayer.Source = string.Empty;
			if (_nativePlayer != null)
				_nativePlayer.Release = true;
			IPlatform platform = DependencyService.Get<IPlatform>();
			platform.ChangeScreenOrientation(ScreenOrientation.Portrait);
		}
	}
}