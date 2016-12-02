using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using BranchXamarinSDK;
using Exchange.BranchService;
using Exchange.Configs;
using Exchange.Interfaces;
using Exchange.Models;
using Exchange.Pages.Base;
using Exchange.Services;
using Xamarin.Forms;
using System.Linq;
using Exchange.Controls.VideoPlayer;

namespace Exchange.Pages
{
	public partial class ExchangeDetailPage : BaseDetailPage<Video>,
										IDeepLinkPage<Video>,
										IBranchUrlInterface
	{
		private string _likeId;
		private IUser _currentUser;
		protected ObservableCollection<Comment> Comments;

		#region Constructors

		public ExchangeDetailPage()
		{
			Content = Loading;
		}

		public ExchangeDetailPage(string askId) : base(askId)
		{
			Content = Loading;
		}

		public ExchangeDetailPage(Video ask) : base(ask)
		{
			Content = Loading;
		}

		#endregion

		protected override void Init()
		{
			Comments = new ObservableCollection<Comment>();
			_repliesListview.ItemsSource = Comments;
			base.Init();
		}

		protected override async Task LoadData(string id)
		{
			await base.LoadData(id);

			_currentUser = await CustomUserManager.Instance.GetCurrentUser();
			if (Model.Likes != null)
			{
				if (Model.Likes.Any((i => i.Value.ObjectId == _currentUser.ObjectId)))
				{
					_likeId = Model.Likes.Where((i => i.Value.ObjectId == _currentUser.ObjectId)).FirstOrDefault().Key;
					_likeButton.Style = Styles.LikedButton;
				}
			}
			else
			{
				_likeId = string.Empty;
			}
			Content = MainLayout;

			_thumbnailImage.Source = Model.Thumbnail;
			_titleLabel.Text = Model.Title;
			if (Model.Likes != null)
				_likesLabel.Text = Model.Likes.Count.ToString();

			if (Model.User != null)
				_subtitleLabel.Text = Model.User.DisplayName + "\n" + Model.CreatedAt.HumanDate();
			_descriptionLabel.Text = Model.Description;
			await LoadReplies();
		}

		private async Task LoadReplies()
		{
			_repliesLayout.Children.Insert(0, Loading);
			List<Comment> comments = await VideoService.Instance.GetReplies(Model);
			if (comments != null)
				foreach (var comment in comments)
					Comments.Add(comment);

			_responsesLabel.Text = Comments.Count + " Respuestas";

			_repliesLayout.Children.Remove(Loading);
		}

		protected override async Task<Video> RetriveDataById(string id)
		{
			if (!string.IsNullOrEmpty(id))
				return await VideoService.Instance.Get(id);
			return new Video();
		}

		#region Listeners
		private void PlayVideo(object sender, EventArgs e)
		{
			LauncheVideoPlayer(Model);
		}

		private async void LikeButton_Clicked(object sender, EventArgs e)
		{
			((Button)sender).IsEnabled = false;

			if (string.IsNullOrEmpty(_likeId))
			{
				_likeId = await VideoService.Instance.Like(Model, new User(_currentUser));
				_likesLabel.Text = (int.Parse(_likesLabel.Text) + 1).ToString();
				_likeButton.Style = Styles.LikedButton;
			}
			else
			{
				bool success = await VideoService.Instance.DeleteLike(Model, _likeId);
				if (success)
				{
					_likeId = string.Empty;
					_likesLabel.Text = (int.Parse(_likesLabel.Text) - 1).ToString();
					_likeButton.Style = Styles.NonLikedButton;
				}
			}
			((Button)sender).IsEnabled = true;
		}

		private void RepliesListview_ItemSelected(object sender, SelectedItemChangedEventArgs e)
		{
			if (e.SelectedItem == null)
				return;

			_repliesListview.SelectedItem = null;
		}

		private void ShareToolbarItem_Clicked(object sender, EventArgs e)
		{
			var data = new Dictionary<string, object>();
			data.Add("objectId", Model.ObjectId);
			var deepLinkMetadata = new DeepLinkMetadata
			{
				Title = "ExchangeUDB",
				Description = "Descarga la app",
				ImageUrl = "https://raw.githubusercontent.com/rejbarrera/ExchangeUDB/master/Droid/Resources/drawable-mdpi/ic_exchange_logo.png",

			};
			//BranchService<Video, VideoDetailPage>.Instance.GenerateLink(data, deepLinkMetadata);
		}

		private void ReplyFab_Clicked(object sender, EventArgs e)
		{
			Navigation.PushAsync(new EditComment(PostComment));
		}

		private async void PostComment(Comment comment)
		{
			if (comment != null && !string.IsNullOrEmpty(comment.Message))
			{
				_repliesLayout.Children.Insert(0, Loading);

				await VideoService.Instance.Reply(Model, comment);
				comment.User.DisplayName = _currentUser.DisplayName;
				comment.User.ProfilePicture = _currentUser.ProfilePicture;
				Comments.Insert(0, comment);
				_responsesLabel.Text = Comments.Count + " Respuestas";

				_repliesLayout.Children.Remove(Loading);

			}
		}

		#endregion

		#region IBranchUrlInterface implementation

		public void ReceivedUrl(string uri)
		{
			throw new NotImplementedException();
		}

		public void UrlRequestError(BranchError error)
		{
			throw new NotImplementedException();
		}

		#endregion

		#region IDeepLinkPage

		public Task OnDeeplink(Video model)
		{
			throw new NotImplementedException();
		}

		#endregion

		#region Player setup
		private void LauncheVideoPlayer(Video model)
		{
			var videoPlayer = new VideoPlayerPage(
				new VideoPlayerAsset(model.Url,
									 StreamingFormat.Hls,
									 model.Url,
									 null,
									 null),
				new VideoPlayerListener(model));
			Navigation.PushModalAsync(videoPlayer);
		}

		private class VideoPlayerAsset : IVideoPlayerAsset
		{
			private string _id;
			public string Id
			{
				get { return _id; }
				private set { _id = value; }
			}

			private string _keyDeliveryUrl;
			public string KeyDeliveryUrl
			{
				get { return _keyDeliveryUrl; }
				private set { _keyDeliveryUrl = value; }
			}

			private StreamingFormat _streamingFormat;
			public StreamingFormat StreamingFormat
			{
				get { return _streamingFormat; }
				private set { _streamingFormat = value; }
			}

			private string _token;
			public string Token
			{
				get { return _token; }
				private set { _token = value; }
			}

			private string _videoUrl;
			public string VideoUrl
			{
				get { return _videoUrl; }
				private set { _videoUrl = value; }
			}

			public PlayerOrientation PlayerOrientation
			{
				get { return PlayerOrientation.Landscape; }
			}

			public VideoPlayerAsset(string videoUrl, StreamingFormat streamingFormat = StreamingFormat.Hls, string id = null,
									string keyDeliveryUrl = null, string token = null)
			{
				_videoUrl = videoUrl;
				_id = id;
				_keyDeliveryUrl = keyDeliveryUrl;
				_streamingFormat = streamingFormat;
				_token = token;
			}
		}

		private class VideoPlayerListener : IVideoPlayerListener
		{
			Video _model;
			public VideoPlayerListener(Video model)
			{
				_model = model;
			}

			Action<PlaybackStates> IVideoPlayerListener
				.OnPlaybackStateChange => OnPlaybackStateChange;

			public void OnPlaybackStateChange(PlaybackStates playbackState)
			{
				string state;

				switch (playbackState)
				{
					case PlaybackStates.StateIdle:
						state = "idle";
						break;
					case PlaybackStates.StatePreparing:
						state = "preparing";
						break;
					case PlaybackStates.StateBuffering:
						state = "buffering";
						break;
					case PlaybackStates.StateReady:
						state = "ready";
						break;
					case PlaybackStates.StateEnded:
						state = "ended";
						break;
					default:
						state = "unknown";
						break;
				}
				System.Diagnostics.Debug.WriteLine(state);
			}
		}
		#endregion
	}
}


