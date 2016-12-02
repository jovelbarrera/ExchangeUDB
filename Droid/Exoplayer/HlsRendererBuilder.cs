using Android.Content;
using Android.Media;
using Android.OS;
using Android.Runtime;
using Com.Google.Android.Exoplayer;
using Com.Google.Android.Exoplayer.Audio;
using Com.Google.Android.Exoplayer.Hls;
using Com.Google.Android.Exoplayer.Text;
using Com.Google.Android.Exoplayer.Text.Eia608;
using Com.Google.Android.Exoplayer.Upstream;
using Com.Google.Android.Exoplayer.Util;
using Java.IO;
using Java.Lang;

namespace Exchange.Droid.Exoplayer
{
	public class HlsRendererBuilder : ExoPlayer.IRendererBuilder
	{
		private const int BufferSegmentSize = 64 * 1024;
		private const int MainBufferSegments = 254;
		private const int AudioBufferSegments = 54;
		private const int TextBufferSegments = 2;

		private Context _context;
		private string _userAgent;
		private string _url;

		private string _keyDeliveryUrl;
		private string _token;

		private AsyncRendererBuilder _currentAsyncBuilder;

		public HlsRendererBuilder(Context context, string userAgent, string url, string keyDeliveryUrl, string token)
		{
			_context = context;
			_userAgent = userAgent;
			_url = url;
			_keyDeliveryUrl = keyDeliveryUrl;
			_token = token;
		}

		public void BuildRenderers(ExoPlayer player)
		{
			_currentAsyncBuilder = new AsyncRendererBuilder(_context, _userAgent, _url, player, _keyDeliveryUrl, _token);
			_currentAsyncBuilder.Init();
		}

		public void Cancel()
		{
			if (_currentAsyncBuilder != null)
			{
				_currentAsyncBuilder.Cancel();
				_currentAsyncBuilder = null;
			}
		}

		private class AsyncRendererBuilder : Object, ManifestFetcher.IManifestCallback
		{
			private Context _context;
			private string _userAgent;
			private ExoPlayer _player;
			private ManifestFetcher _playlistFetcher;
			private bool _canceled;

			private string _keyDeliveryUrl;
			private string _token;

			public AsyncRendererBuilder(Context context, string userAgent, string url, ExoPlayer player, string keyDeliveryUrl, string token)
			{
				_context = context;
				_userAgent = userAgent;
				_player = player;
				HlsPlaylistParser parser = new HlsPlaylistParser();
				_playlistFetcher = new ManifestFetcher(url, new DefaultUriDataSource(context, userAgent), parser);
				_keyDeliveryUrl = keyDeliveryUrl;
				_token = token;
			}

			public void Init()
			{
				_playlistFetcher.SingleLoad(_player.MainHandler.Looper, this);
			}

			public void Cancel()
			{
				_canceled = true;
			}

			public void OnSingleManifestError(IOException e)
			{
				if (_canceled)
				{
					return;
				}

				_player.OnRenderersError(e);
			}

			public void OnSingleManifest(Object obj)
			{
				var manifest = obj.JavaCast<HlsPlaylist>();
				if (_canceled)
					return;

				Handler mainHandler = _player.MainHandler;
				ILoadControl loadControl = new DefaultLoadControl(new DefaultAllocator(BufferSegmentSize));
				DefaultBandwidthMeter bandwidthMeter = new DefaultBandwidthMeter();
				PtsTimestampAdjusterProvider timestampAdjusterProvider = new PtsTimestampAdjusterProvider();

				bool haveSubtitles = false;
				bool haveAudios = false;
				if (manifest is HlsMasterPlaylist)
				{
					HlsMasterPlaylist masterPlaylist = (HlsMasterPlaylist)manifest;
					haveSubtitles = masterPlaylist.Subtitles.Count != 0;
					haveAudios = masterPlaylist.Audios.Count != 0;
				}

				// Build the video/id3 renderers.
				IDataSource dataSource = new Aes128DataSource(_context, bandwidthMeter, _userAgent, _keyDeliveryUrl, _token);
				HlsChunkSource chunkSource = new HlsChunkSource(true /* isMaster */, dataSource, manifest,
					DefaultHlsTrackSelector.NewDefaultInstance(_context), bandwidthMeter,
																timestampAdjusterProvider, HlsChunkSource.AdaptiveModeSplice);
				HlsSampleSource sampleSource = new HlsSampleSource(chunkSource, loadControl,
																   MainBufferSegments * BufferSegmentSize,
																   mainHandler,
																   _player,
																   ExoPlayer.TypeVideo);
				MediaCodecVideoTrackRenderer videoRenderer = new MediaCodecVideoTrackRenderer(_context,
																							  sampleSource,
																							  MediaCodecSelector.Default,
																							  (int)VideoScalingMode.ScaleToFit,
																							  //MediaCodec.VideoScalingModeScaleToFit,
																							  5000,
																							  mainHandler,
																							  _player, 50);

				//MetadataTrackRenderer id3Renderer = new MetadataTrackRenderer(sampleSource, new Id3Parser(), player, mainHandler.Looper);

				// Build the audio renderer.
				MediaCodecAudioTrackRenderer audioRenderer;
				if (haveAudios)
				{
					IDataSource audioDataSource = new Aes128DataSource(_context, bandwidthMeter, _userAgent, _keyDeliveryUrl, _token);
					HlsChunkSource audioChunkSource = new HlsChunkSource(false /* isMaster */, audioDataSource,
						manifest, DefaultHlsTrackSelector.NewAudioInstance(), bandwidthMeter,
						 timestampAdjusterProvider, HlsChunkSource.AdaptiveModeSplice);
					HlsSampleSource audioSampleSource = new HlsSampleSource(audioChunkSource, loadControl,
						AudioBufferSegments * BufferSegmentSize, mainHandler, _player,
																			ExoPlayer.TypeAudio);
					audioRenderer = new MediaCodecAudioTrackRenderer(
						new ISampleSource[] { sampleSource, audioSampleSource }, MediaCodecSelector.Default, null,
						true, _player.MainHandler, _player, AudioCapabilities.GetCapabilities(_context),
						(int)Stream.Music);
				}
				else
				{
					audioRenderer = new MediaCodecAudioTrackRenderer(sampleSource,
						 MediaCodecSelector.Default, null, true, _player.MainHandler, _player,
						AudioCapabilities.GetCapabilities(_context), (int)Stream.Music);
				}

				// Build the text renderer.
				TrackRenderer textRenderer;
				if (haveSubtitles)
				{
					IDataSource textDataSource = new Aes128DataSource(_context, bandwidthMeter, _userAgent, _keyDeliveryUrl, _token);
					HlsChunkSource textChunkSource = new HlsChunkSource(false /* isMaster */, textDataSource,
						manifest, DefaultHlsTrackSelector.NewSubtitleInstance(), bandwidthMeter,
																		timestampAdjusterProvider, HlsChunkSource.AdaptiveModeSplice);
					HlsSampleSource textSampleSource = new HlsSampleSource(textChunkSource, loadControl,
																		   TextBufferSegments * BufferSegmentSize, mainHandler, _player, ExoPlayer.TypeText);
					textRenderer = new TextTrackRenderer(textSampleSource, _player, mainHandler.Looper);
				}
				else
				{
					textRenderer = new Eia608TrackRenderer(sampleSource, _player, mainHandler.Looper);
				}

				TrackRenderer[] renderers = new TrackRenderer[ExoPlayer.RendererCount];
				renderers[ExoPlayer.TypeVideo] = videoRenderer;
				renderers[ExoPlayer.TypeAudio] = audioRenderer;
				//renderers[VideoPlayer.TypeMetadata] = id3Renderer;
				renderers[ExoPlayer.TypeText] = textRenderer;
				_player.OnRenderers(renderers, bandwidthMeter);
			}
		}
	}
}