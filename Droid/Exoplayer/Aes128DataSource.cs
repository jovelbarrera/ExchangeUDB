using Android.Content;
using Com.Google.Android.Exoplayer.Upstream;
using Com.Google.Android.Exoplayer.Util;
using Java.Lang;

namespace Exchange.Droid.Exoplayer
{
	public sealed class Aes128DataSource : Object, IDataSource
	{
		private const string SchemeAsset = "asset";
		private const string SchemeContent = "content";

		private IUriDataSource _httpDataSource;
		private IUriDataSource _fileDataSource;
		private IUriDataSource _assetDataSource;
		private IUriDataSource _contentDataSource;
		private string _keyDeliveryUrl;
		private string _token;

		private IUriDataSource _dataSource;

		public string Uri { get { return _dataSource == null ? null : _dataSource.Uri; } }

		public Aes128DataSource(Context context, string userAgent, string keyDeliveryUrl, string token)
		{
			Init(context, null, userAgent, false, keyDeliveryUrl, token);
		}

		public Aes128DataSource(Context context, ITransferListener listener, string userAgent,
								string keyDeliveryUrl, string token)
		{
			Init(context, listener, userAgent, false, keyDeliveryUrl, token);
		}

		public void Init(Context context, ITransferListener listener, string userAgent,
						 bool allowCrossProtocolRedirects, string keyDeliveryUrl, string token)
		{
			_keyDeliveryUrl = keyDeliveryUrl;
			_token = token;

			var httpDataSource = new DefaultHttpDataSource(userAgent, null, listener,
			  DefaultHttpDataSource.DefaultConnectTimeoutMillis,
			  DefaultHttpDataSource.DefaultReadTimeoutMillis, allowCrossProtocolRedirects);

			if (httpDataSource == null)
				throw new NullPointerException();

			_httpDataSource = httpDataSource;
			_fileDataSource = new FileDataSource(listener);
			_assetDataSource = new AssetDataSource(context, listener);
			_contentDataSource = new ContentDataSource(context, listener);
		}

		public long Open(DataSpec dataSpec)
		{
			Assertions.CheckState(_dataSource == null);
			// Choose the correct source for the scheme.
			string scheme = dataSpec.Uri.Scheme;
			var url = Android.Net.Uri.Parse(dataSpec.Uri.ToString());
			if (Com.Google.Android.Exoplayer.Util.ExoPlayerUtil.IsLocalFileUri(url))
			{
				if (dataSpec.Uri.Path.StartsWith("/android_asset/"))
				{
					_dataSource = _assetDataSource;
				}
				else {
					_dataSource = _fileDataSource;
				}
			}
			else if (SchemeAsset.Equals(scheme))
			{
				_dataSource = _assetDataSource;
			}
			else if (SchemeContent.Equals(scheme))
			{
				_dataSource = _contentDataSource;
			}
			else
			{
				_dataSource = _httpDataSource;
			}
			string currentUrl = dataSpec.Uri.ToString();
			if (_keyDeliveryUrl != null && _token != null && currentUrl.Contains(_keyDeliveryUrl))
				dataSpec = new DataSpec(Android.Net.Uri.Parse(string.Format("{0}&token={1}", _keyDeliveryUrl, _token)));
			try
			{
				return _dataSource.Open(dataSpec);
			}
			catch (Exception ex)
			{
				return 0;
			}
		}

		public int Read(byte[] buffer, int offset, int readLength)
		{
			try
			{
				return _dataSource.Read(buffer, offset, readLength);
			}
			catch (Exception ex)
			{
				return 0;
			}
		}

		public void Close()
		{
			if (_dataSource != null)
			{
				try
				{
					_dataSource.Close();
				}
				finally
				{
					_dataSource = null;
				}
			}
		}
	}
}