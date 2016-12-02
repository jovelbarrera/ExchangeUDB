using System;
using Android.App;
using Android.Text;
using Android.Views;
using Android.Widget;
using Com.Google.Android.Exoplayer;
using Com.Google.Android.Exoplayer.Audio;
using Com.Google.Android.Exoplayer.Text;
using Com.Google.Android.Exoplayer.Util;
using Exchange.Droid.Exoplayer;
using Java.Net;
using Java.Util;
using Xamarin.Forms.Platform.Android;
using String = Java.Lang.String;

namespace Exchange.Droid.DroidVideoPlayer
{
	// UI Definition
	public partial class DroidVideoPlayer
	{
		private EventLogger _eventLogger;
		private MediaController _mediaController;

		//private TextView _debugTextView;
		//private TextView _playerStateTextView;

		private AspectRatioFrameLayout _videoFrame;
		private SurfaceView _surfaceView;
		private SubtitleLayout _subtitleLayout;
		private View _shutterView;

		//private View _debugRootView;
		//private Button _videoButton;
		//private Button _audioButton;
		//private Button _textButton;
		//private Button _retryButton;

		private View _mainView;

		#region Activity lifecycle

		protected void InitPlayer()
		{
			Activity activity = Context as Activity;
			_mainView = activity.LayoutInflater.Inflate(Resource.Layout.VideoPlayer, this, false);

			var root = _mainView.FindViewById(Resource.Id.root);
			root.Touch += OnTouch;
			root.KeyPress += OnKeyPres;

			#region Player Frame
			_videoFrame = _mainView.FindViewById<AspectRatioFrameLayout>(Resource.Id.video_frame);
			_surfaceView = _mainView.FindViewById<SurfaceView>(Resource.Id.surface_view);
			_surfaceView.Holder.AddCallback(this);
			_shutterView = _mainView.FindViewById(Resource.Id.shutter);
			_subtitleLayout = _mainView.FindViewById<SubtitleLayout>(Resource.Id.subtitles);
			#endregion

			#region DebugInfo
			//_playerStateTextView = _mainView.FindViewById<TextView>(Resource.Id.player_state_view);
			//_debugTextView = _mainView.FindViewById<TextView>(Resource.Id.debug_text_view);
			#endregion

			#region TopButtons
			//_debugRootView = _mainView.FindViewById(Resource.Id.controls_root);

			//_videoButton = _mainView.FindViewById<Button>(Resource.Id.video_controls);
			//_videoButton.Click += ShowVideoPopup;

			//_audioButton = _mainView.FindViewById<Button>(Resource.Id.audio_controls);
			//_audioButton.Click += ShowAudioPopup;

			//_textButton = _mainView.FindViewById<Button>(Resource.Id.text_controls);
			//_textButton.Click += ShowTextPopup;

			//_retryButton = _mainView.FindViewById<Button>(Resource.Id.retry_button);
			//_retryButton.SetOnClickListener(this);
			#endregion

			_mediaController = new MediaController(Context);
			_mediaController.SetAnchorView(root);

			var currentHandler = CookieHandler.Default;
			if (currentHandler != DefaultCookieManager)
			{
				CookieHandler.Default = DefaultCookieManager;
			}

			_audioCapabilitiesReceiver = new AudioCapabilitiesReceiver(Context, this);
			_audioCapabilitiesReceiver.Register();

			ConfigureSubtitleView();
			if (_player == null)
			{
				PreparePlayer(true);
			}
			else
			{
				_player.Backgrounded = false;
			}
		}

		public void OnResume()
		{
			ConfigureSubtitleView();
			if (_player == null)
			{
				PreparePlayer(true);
			}
			else
			{
				_player.Backgrounded = false;
			}
		}

		public void OnPause()
		{
			if (!_enableBackgroundAudio)
			{
				ReleasePlayer();
			}
			else
			{
				_player.Backgrounded = true;
			}
			_shutterView.Visibility = ViewStates.Visible;
		}

		public void OnDestroy()
		{
			_audioCapabilitiesReceiver.Unregister();
			ReleasePlayer();
		}

		#endregion

		#region User controls

		private void OnTouch(object sender, TouchEventArgs e)
		{
			var motionEvent = e.Event;
			switch (motionEvent.Action)
			{
				case MotionEventActions.Down:
					ToggleControlsVisibility();
					break;
				case MotionEventActions.Up:
					((View)sender).PerformClick();
					break;
			}
			e.Handled = true;
		}

		private void OnKeyPres(object sender, KeyEventArgs e)
		{
			var keyCode = e.KeyCode;
			if (keyCode == Keycode.Back || keyCode == Keycode.Escape
				|| keyCode == Keycode.Menu)
			{
				e.Handled = false;
			}
			else
			{
				_mediaController.DispatchKeyEvent(e.Event);
			}
		}

		private void UpdateButtonVisibilities()
		{
			//_retryButton.Visibility = _playerNeedsPrepare ? ViewStates.Visible : ViewStates.Gone;
			//_videoButton.Visibility = HaveTracks(Exoplayer.ExoPlayer.TypeVideo) ? ViewStates.Visible : ViewStates.Gone;
			//_audioButton.Visibility = HaveTracks(Exoplayer.ExoPlayer.TypeAudio) ? ViewStates.Visible : ViewStates.Gone;
			//_textButton.Visibility = HaveTracks(Exoplayer.ExoPlayer.TypeText) ? ViewStates.Visible : ViewStates.Gone;
		}

		private bool HaveTracks(int type)
		{
			return _player != null && _player.GetTrackCount(type) > 0;
		}

		public void ShowVideoPopup(object sender, EventArgs e)
		{
			var popup = new PopupMenu(Context, (View)sender);
			ConfigurePopupWithTracks(popup, null, Exoplayer.ExoPlayer.TypeVideo);
			popup.Show();
		}

		public void ShowAudioPopup(object sender, EventArgs e)
		{
			var popup = new PopupMenu(Context, (View)sender);
			var menu = popup.Menu;
			menu.Add(Menu.None, Menu.None, Menu.None, Resource.String.enable_background_audio);
			var backgroundAudioItem = menu.FindItem(0);
			backgroundAudioItem.SetCheckable(true);
			backgroundAudioItem.SetChecked(_enableBackgroundAudio);

			Func<IMenuItem, bool> clickListener = item =>
			{
				if (item == backgroundAudioItem)
				{
					_enableBackgroundAudio = !item.IsChecked;
					return true;
				}
				return false;
			};
			ConfigurePopupWithTracks(popup, clickListener, Exoplayer.ExoPlayer.TypeAudio);
			popup.Show();
		}

		public void ShowTextPopup(object sender, EventArgs e)
		{
			var popup = new PopupMenu(Context, (View)sender);
			ConfigurePopupWithTracks(popup, null, Exoplayer.ExoPlayer.TypeText);
			popup.Show();
		}

		public void ShowVerboseLogPopup(object sender, EventArgs e)
		{
			var popup = new PopupMenu(Xamarin.Forms.Forms.Context, (View)sender);
			var menu = popup.Menu;
			menu.Add(Menu.None, 0, Menu.None, Resource.String.logging_normal);
			menu.Add(Menu.None, 1, Menu.None, Resource.String.logging_verbose);
			menu.SetGroupCheckable(Menu.None, true, true);
			menu.FindItem((VerboseLogUtil.AreAllTagsEnabled()) ? 1 : 0).SetChecked(true);

			popup.MenuItemClick += (s, a) =>
			{
				var item = a.Item;
				VerboseLogUtil.SetEnableAllTags(item.ItemId != 0);
			};
			popup.Show();
		}

		private void ConfigurePopupWithTracks(PopupMenu popup, Func<IMenuItem, bool> customActionClickListener, int trackType)
		{
			if (_player == null)
			{
				return;
			}
			var trackCount = _player.GetTrackCount(trackType);
			if (trackCount == 0)
			{
				return;
			}

			popup.MenuItemClick += (sender, args) =>
			{
				var item = args.Item;
				args.Handled = (customActionClickListener != null
								&& customActionClickListener(item))
							   || OnTrackItemClick(item, trackType);
			};

			var menu = popup.Menu;
			// ID_OFFSET ensures we avoid clashing with Menu.NONE (which equals 0)
			menu.Add(MenuGroupTracks, Exoplayer.ExoPlayer.TrackDisabled + IdOffset, Menu.None, Resource.String.off);
			for (var i = 0; i < trackCount; i++)
			{
				menu.Add(MenuGroupTracks, i + IdOffset, Menu.None,
					BuildTrackName(_player.GetTrackFormat(trackType, i)));
			}
			menu.SetGroupCheckable(MenuGroupTracks, true, true);
			menu.FindItem(_player.GetSelectedTrack(trackType) + IdOffset).SetChecked(true);
		}

		private static string BuildTrackName(MediaFormat format)
		{
			if (format.Adaptive)
			{
				return "auto";
			}
			string trackName;
			if (MimeTypes.IsVideo(format.MimeType))
			{
				trackName = JoinWithSeparator(JoinWithSeparator(BuildResolutionString(format),
					BuildBitrateString(format)), BuildTrackIdString(format));
			}
			else if (MimeTypes.IsAudio(format.MimeType))
			{
				trackName = JoinWithSeparator(JoinWithSeparator(JoinWithSeparator(BuildLanguageString(format),
					BuildAudioPropertyString(format)), BuildBitrateString(format)),
					BuildTrackIdString(format));
			}
			else
			{
				trackName = JoinWithSeparator(JoinWithSeparator(BuildLanguageString(format),
					BuildBitrateString(format)), BuildTrackIdString(format));
			}
			return trackName.Length == 0 ? "unknown" : trackName;
		}

		private static string BuildResolutionString(MediaFormat format)
		{
			return format.Width == MediaFormat.NoValue || format.Height == MediaFormat.NoValue
				? ""
				: format.Width + "x" + format.Height;
		}

		private static string BuildAudioPropertyString(MediaFormat format)
		{
			return format.ChannelCount == MediaFormat.NoValue || format.SampleRate == MediaFormat.NoValue
				? ""
				: format.ChannelCount + "ch, " + format.SampleRate + "Hz";
		}

		private static string BuildLanguageString(MediaFormat format)
		{
			return TextUtils.IsEmpty(format.Language) || "und".Equals(format.Language)
				? ""
				: format.Language;
		}

		private static string BuildBitrateString(MediaFormat format)
		{
			return format.Bitrate == MediaFormat.NoValue
				? ""
				: String.Format(Locale.Us, "%.2fMbit", format.Bitrate / 1000000f);
		}

		private static string JoinWithSeparator(string first, string second)
		{
			return first.Length == 0 ? second : (second.Length == 0 ? first : first + ", " + second);
		}

		private static string BuildTrackIdString(MediaFormat format)
		{
			return format.TrackId == null
				? ""
				: String.Format(Locale.Us, " (%s)", format.TrackId);
		}

		private bool OnTrackItemClick(IMenuItem item, int type)
		{
			if (_player == null || item.GroupId != MenuGroupTracks)
			{
				return false;
			}
			_player.SetSelectedTrack(type, item.ItemId - IdOffset);
			return true;
		}

		private void ToggleControlsVisibility()
		{
			if (_mediaController.IsShowing)
			{
				_mediaController.Hide();
				//_debugRootView.Visibility = ViewStates.Gone;
			}
			else
			{
				ShowControls();
			}
		}

		private void ShowControls()
		{
			_mediaController.Show(0);
			//_debugRootView.Visibility = ViewStates.Visible;
		}

		#endregion
	}
}