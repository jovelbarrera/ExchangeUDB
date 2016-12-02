using System;
namespace Exchange.Controls.VideoPlayer
{
	public interface IVideoPlayerListener
	{
		Action<PlaybackStates> OnPlaybackStateChange { get; }
	}
}

