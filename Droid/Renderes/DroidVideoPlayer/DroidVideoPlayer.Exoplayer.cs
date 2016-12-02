using System.Collections.Generic;
using Android.Content;
using Android.Graphics;
using Android.Views;
using Android.Views.Accessibility;
using Android.Widget;
using Com.Google.Android.Exoplayer.Audio;
using Com.Google.Android.Exoplayer.Drm;
using Com.Google.Android.Exoplayer.Text;
using Com.Google.Android.Exoplayer.Util;
using Exchange.Controls.VideoPlayer;
using Exchange.Droid.DroidVideoPlayer;
using Exchange.Droid.Exoplayer;
using Java.Lang;
using Java.Net;
using Exception = Java.Lang.Exception;
using Uri = Android.Net.Uri;
using XF = Xamarin.Forms;

[assembly: XF.ExportRenderer(typeof(VideoPlayer), typeof(DroidVideoPlayer))]
namespace Exchange.Droid.DroidVideoPlayer
{
public partial class DroidVideoPlayer : View.IOnClickListener,
									ExoPlayer.IListener,
									ExoPlayer.ICaptionListener,
									ExoPlayer.ID3MetadataListener,
									AudioCapabilitiesReceiver.IListener
	{
		public IVideoPlayerAsset Asset;
		public IVideoPlayerListener Listener;

		private const string ExtDash = ".mpd";
		private const string ExtSs = ".ism";
		private const string ExtHls = ".m3u8";
		private const int MenuGroupTracks = 1;
		private const int IdOffset = 2;

		private static readonly CookieManager DefaultCookieManager;

		private ExoPlayer _player;
		//private DebugTextViewHelper _debugViewHelper;
		private bool _playerNeedsPrepare;
		private long _playerPosition;
		private bool _enableBackgroundAudio;

		private AudioCapabilitiesReceiver _audioCapabilitiesReceiver;

		static DroidVideoPlayer()
		{
			DefaultCookieManager = new CookieManager();
			DefaultCookieManager.SetCookiePolicy(CookiePolicy.AcceptOriginalServer);
		}

		#region OnClickListener methods

		public void OnClick(View view)
		{
			//if (view == _retryButton)
			//{
			//	PreparePlayer(true);
			//}
		}

		#endregion

		#region AudioCapabilitiesReceiver.Listener methods

		public void OnAudioCapabilitiesChanged(AudioCapabilities audioCapabilities)
		{
			if (_player == null)
				return;

			var backgrounded = _player.Backgrounded;
			var playWhenReady = _player.PlayWhenReady;
			ReleasePlayer();
			PreparePlayer(playWhenReady);
			_player.Backgrounded = backgrounded;
		}

		#endregion

		#region Internal methods

		private ExoPlayer.IRendererBuilder GetRendererBuilder()
		{
			var userAgent = ExoPlayerUtil.GetUserAgent(Context, "ExoPlayer");
			switch (Asset.StreamingFormat)
			{
				//case TypeSs:
				//	return new SmoothStreamingRendererBuilder(this, userAgent, _contentUri.ToString(),
				//		new SmoothStreamingTestMediaDrmCallback());
				//case TypeDash:
				//	return new DashRendererBuilder(this, userAgent, _contentUri.ToString(),
				//		new WidevineTestMediaDrmCallback(_contentId));
				case StreamingFormat.Hls:
					return new HlsRendererBuilder(Context, userAgent, Asset.VideoUrl, Asset.KeyDeliveryUrl, Asset.Token);
				//case TypeOther:
				//	return new ExtractorRendererBuilder(this, userAgent, _contentUri);
				default:
					throw new IllegalStateException("Unsupported type: " + Asset.StreamingFormat.ToString());
			}
		}

		private void PreparePlayer(bool playWhenReady)
		{
			if (_player == null)
			{
				_player = new Exoplayer.ExoPlayer(GetRendererBuilder());
				_player.AddListener(this);
				_player.SetCaptionListener(this);
				_player.SetMetadataListener(this);
				_player.SeekTo(_playerPosition);
				_playerNeedsPrepare = true;
				_mediaController.SetMediaPlayer(_player.PlayerControl);
				_mediaController.Enabled = true;
				_eventLogger = new EventLogger();
				_eventLogger.StartSession();
				_player.AddListener(_eventLogger);
				_player.SetInfoListener(_eventLogger);
				_player.SetInternalErrorListener(_eventLogger);
				//_debugViewHelper = new DebugTextViewHelper(_player, _debugTextView);
				//_debugViewHelper.Start();
			}
			if (_playerNeedsPrepare)
			{
				_player.Prepare();
				_playerNeedsPrepare = false;
				UpdateButtonVisibilities();
			}
			_player.Surface = _surfaceView.Holder.Surface;
			_player.PlayWhenReady = playWhenReady;
		}

		private void ReleasePlayer()
		{
			if (_player != null)
			{
				//_debugViewHelper.Stop();
				//_debugViewHelper = null;
				_playerPosition = _player.CurrentPosition;
				_player.Release();
				_player = null;
				_eventLogger.EndSession();
				_eventLogger = null;
			}
		}

		#endregion

		#region VideoPlayer.Listener implementation

		public void OnStateChanged(bool playWhenReady, int playbackState)
		{
			if (playbackState == Com.Google.Android.Exoplayer.ExoPlayer.StateEnded)
			{
				ShowControls();
			}
			var text = "playWhenReady=" + playWhenReady + ", playbackState=";
			switch (playbackState)
			{
				case Com.Google.Android.Exoplayer.ExoPlayer.StateBuffering: // 3
					text += "buffering";
					break;
				case Com.Google.Android.Exoplayer.ExoPlayer.StateEnded: // 5
					text += "ended";
					break;
				case Com.Google.Android.Exoplayer.ExoPlayer.StateIdle: // 1
					text += "idle";
					break;
				case Com.Google.Android.Exoplayer.ExoPlayer.StatePreparing: // 2
					text += "preparing";
					break;
				case Com.Google.Android.Exoplayer.ExoPlayer.StateReady: // 4
					text += "ready";
					break;
				default:
					text += "unknown";
					break;
			}
			//_playerStateTextView.Text = text;
			UpdateButtonVisibilities();
			if (Listener != null)
				Listener.OnPlaybackStateChange((PlaybackStates)playbackState);
		}

		public void OnError(Exception e)
		{
			var exception = e as UnsupportedDrmException;
			if (exception != null)
			{
				// Special case DRM failures.
				var stringId = ExoPlayerUtil.SdkInt < 18
					? Resource.String.drm_error_not_supported
					: exception.Reason == UnsupportedDrmException.ReasonUnsupportedScheme
						? Resource.String.drm_error_unsupported_scheme
						: Resource.String.drm_error_unknown;
				Toast.MakeText(Context, stringId, ToastLength.Long).Show();
			}
			_playerNeedsPrepare = true;
			UpdateButtonVisibilities();
			ShowControls();
		}

		public void OnVideoSizeChanged(int width, int height, int unappliedRotationDegrees, float pixelWidthAspectRatio)
		{
			_shutterView.Visibility = ViewStates.Gone;
			_videoFrame.SetAspectRatio(height == 0 ? 1 : (width * pixelWidthAspectRatio) / height);
		}

		#endregion

		#region VidePlayer.CaptionListener implementation

		public void OnCues(IList<Cue> cues)
		{
			_subtitleLayout.SetCues(cues);
		}

		#endregion

		#region VideoPlayer.MetadataListener implementation

		public void OnId3Metadata(object metadata)
		{
		}

		#endregion

		#region Misc
		private void ConfigureSubtitleView()
		{
			CaptionStyleCompat style;
			float fontScale;
			if (ExoPlayerUtil.SdkInt >= 19)
			{
				style = GetUserCaptionStyleV19();
				fontScale = GetUserCaptionFontScaleV19();
			}
			else
			{
				style = CaptionStyleCompat.Default;
				fontScale = 1.0f;
			}
			_subtitleLayout.SetStyle(style);
			_subtitleLayout.SetFractionalTextSize(SubtitleLayout.DefaultTextSizeFraction * fontScale);
		}

		private float GetUserCaptionFontScaleV19()
		{
			var captioningManager =
				(CaptioningManager)Context.GetSystemService(Context.CaptioningService);
			return captioningManager.FontScale;
		}

		private CaptionStyleCompat GetUserCaptionStyleV19()
		{
			var captioningManager =
				(CaptioningManager)Context.GetSystemService(Context.CaptioningService);
			return CaptionStyleCompat.CreateFromCaptionStyle(captioningManager.UserStyle);
		}

		//private static int InferContentType(Uri uri, string fileExtension)
		//{
		//	var lastPathSegment = !string.IsNullOrEmpty(fileExtension)
		//		? "." + fileExtension
		//		: uri.LastPathSegment;
		//	if (lastPathSegment == null)
		//	{
		//		return (int)StreamingFormat.Other;
		//	}
		//	if (lastPathSegment.EndsWith(ExtDash))
		//	{
		//		return (int)StreamingFormat.Dash;
		//	}
		//	if (lastPathSegment.EndsWith(ExtSs))
		//	{
		//		return (int)StreamingFormat.Ss;
		//	}
		//	if (lastPathSegment.EndsWith(ExtHls))
		//	{
		//		return (int)StreamingFormat.Hls;
		//	}
		//	return (int)StreamingFormat.Other;
		//}

		private class KeyCompatibleMediaController : MediaController
		{
			private IMediaPlayerControl _playerControl;

			public KeyCompatibleMediaController(Context context) : base(context)
			{
			}

			public override void SetMediaPlayer(IMediaPlayerControl playerControl)
			{
				base.SetMediaPlayer(playerControl);
				_playerControl = playerControl;
			}

			public override bool DispatchKeyEvent(KeyEvent ev)
			{
				var keyCode = ev.KeyCode;
				if (_playerControl.CanSeekForward() && (keyCode == Keycode.MediaFastForward || keyCode == Keycode.DpadRight))
					if (_playerControl.CanSeekForward() && keyCode == Keycode.MediaFastForward)
					{
						if (ev.Action == KeyEventActions.Down)
						{
							_playerControl.SeekTo(_playerControl.CurrentPosition + 15000); // milliseconds
							Show();
						}
						return true;
					}
					else if (_playerControl.CanSeekBackward() && (keyCode == Keycode.MediaRewind || keyCode == Keycode.DpadLeft))
					{
						if (ev.Action == KeyEventActions.Down)
						{
							_playerControl.SeekTo(_playerControl.CurrentPosition - 5000); // milliseconds
							Show();
						}
						return true;
					}
				return base.DispatchKeyEvent(ev);
			}
		}
		#endregion
	}
}