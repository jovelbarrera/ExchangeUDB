using System;
namespace Exchange.Controls.VideoPlayer
{
	public interface IVideoPlayerAsset
	{
		string Id { get; }
		string VideoUrl { get; }
		string KeyDeliveryUrl { get; }
		string Token { get; }
		StreamingFormat StreamingFormat { get; }
		PlayerOrientation PlayerOrientation { get; }
	}
}

