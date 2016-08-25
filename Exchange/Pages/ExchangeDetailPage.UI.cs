using System;
using Exchange.ViewCells;
using Exchange.Configs;
using Exchange.Controls;
using Xamarin.Forms;

namespace Exchange.Pages
{
	public partial class ExchangeDetailPage : ContentPage
	{
		private StackLayout _mainLayout;
		private Image _thumbnailImage;
		private Label _headerLabel;
		private Label _likesLabel;
		private Label _titleLabel;
		private Label _subtitleLabel;
		private Label _descriptionLabel;
		private Label _responsesLabel;
		private ListView _commentsList;
		private CustomEditor _replyEditor;

		public void InitializeComponents()
		{
			Title = "Exchanges";
			BackgroundColor = Color.White;

			var scrollLayout = new StackLayout();

			#region thumbnail
			var thumbnailLayout = new RelativeLayout
			{
				HeightRequest = 200,
			};
			scrollLayout.Children.Add(thumbnailLayout);

			#region Background
			_thumbnailImage = new Image
			{
				Aspect = Aspect.AspectFill,
			};
			thumbnailLayout.Children.Add(_thumbnailImage,
				Constraint.Constant(0),
				Constraint.Constant(0),
				Constraint.RelativeToParent(p => p.Width),
				Constraint.RelativeToParent(p => p.Height)
			);
			thumbnailLayout.Children.Add(new BoxView { BackgroundColor = Color.Black, Opacity = 0.4 },
				Constraint.Constant(0),
				Constraint.Constant(0),
				Constraint.RelativeToParent(p => p.Width),
				Constraint.RelativeToParent(p => p.Height)
			);
			var playImage = new Image
			{
				Source = "ic_play.png",
				Opacity = 0.4,
			};
			thumbnailLayout.Children.Add(playImage,
				Constraint.RelativeToParent(p => p.Width / 2 - 30),
				Constraint.RelativeToParent(p => p.Height / 2 - 30),
				Constraint.Constant(60),
				Constraint.Constant(60)
			);
			#endregion

			#region videoDescription
			var videoDescriptionLayout = new StackLayout
			{
				Spacing = 0,
			};

			_headerLabel = new Label
			{
				Style = Styles.Title,
				TextColor = Color.White,
				LineBreakMode = LineBreakMode.TailTruncation,
			};
			videoDescriptionLayout.Children.Add(_headerLabel);

			var likesLayout = new StackLayout
			{
				Orientation = StackOrientation.Horizontal,
			};
			likesLayout.Children.Add(new Image { Source = "ic_like.png", HeightRequest = 40 });
			_likesLabel = new Label
			{
				Style = Styles.Title,
				TextColor = Color.White,
				Text = "0",
			};
			likesLayout.Children.Add(_likesLabel);
			videoDescriptionLayout.Children.Add(likesLayout);

			var container = new StackLayout
			{
				Children = { videoDescriptionLayout },
			};
			thumbnailLayout.Children.Add(container,
				Constraint.Constant(20),
				Constraint.RelativeToParent(p => p.Height - 100),
				Constraint.RelativeToParent(p => p.Width * 0.8),
				Constraint.Constant(100)
			);
			#endregion
			#endregion

			#region comments

			var commentsLayout = new StackLayout
			{
				Padding = new Thickness(20),
			};
			scrollLayout.Children.Add(commentsLayout);

			var headerLayout = new StackLayout
			{
				Orientation = StackOrientation.Horizontal,
			};
			commentsLayout.Children.Add(headerLayout);

			var titleLayout = new StackLayout
			{
				HorizontalOptions = LayoutOptions.FillAndExpand,
			};
			headerLayout.Children.Add(titleLayout);
			_titleLabel = new Label
			{
				Style = Styles.Title,
			};
			titleLayout.Children.Add(_titleLabel);

			_subtitleLabel = new Label
			{
				Style = Styles.Subtitle,
			};
			titleLayout.Children.Add(_subtitleLabel);

			var likeLayout = new StackLayout
			{
				VerticalOptions = LayoutOptions.Start,
				HorizontalOptions = LayoutOptions.End,
			};
			headerLayout.Children.Add(likeLayout);
			var likeButton = new Button
			{
				Style = Styles.LikeButton,
				Image = "ic_like_button.png",
				Text = "1+",

			};
			likeLayout.Children.Add(likeButton);

			_descriptionLabel = new Label
			{
				Style = Styles.Verbosa,
			};
			commentsLayout.Children.Add(_descriptionLabel);

			_responsesLabel = new Label
			{
				Style = Styles.Subtitle,
				TextColor = Styles.Colors.NormalText,
			};
			commentsLayout.Children.Add(_responsesLabel);
			#endregion

			_commentsList = new ListView
			{
				VerticalOptions = LayoutOptions.FillAndExpand,
				ItemTemplate = new DataTemplate(typeof(CommentViewCell)),
				HasUnevenRows = true,
			};
			scrollLayout.Children.Add(_commentsList);

			#region reply
			var replyLayout = new StackLayout
			{
				Padding = new Thickness(5),
				BackgroundColor = Styles.Colors.Primary,
				VerticalOptions = LayoutOptions.EndAndExpand,
			};
			//mainLayout.Children.Add(replyLayout);

			var replyContainerLayout = new StackLayout
			{
				Orientation = StackOrientation.Horizontal,
				//BackgroundColor = Color.White,
				Spacing = 15,
			};
			replyLayout.Children.Add(replyContainerLayout);
			var photoButton = new Image
			{
				Source = "ic_photo.png",
			};
			replyContainerLayout.Children.Add(photoButton);
			_replyEditor = new CustomEditor
			{
				Style = Styles.NormalEditor,
				HorizontalOptions = LayoutOptions.FillAndExpand,
				HeightRequest = 40,
			};
			replyContainerLayout.Children.Add(_replyEditor);
			var sendButton = new CustomImage
			{
				//Style = Styles.ActiveButton,
				Source = "ic_send.png",
				//BackgroundColor = Styles.Colors.Primary,
				//Text = "Enviar",
			};
			replyContainerLayout.Children.Add(sendButton);

			#endregion

			_mainLayout = new StackLayout
			{
				Spacing = 0,
				Children ={
					new ScrollView { Content = scrollLayout },
					replyLayout,
				},
			};

			//Content = _mainLayout;
		}
	}
}


