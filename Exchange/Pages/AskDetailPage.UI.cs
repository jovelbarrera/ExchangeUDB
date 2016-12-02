using System;
using Exchange.ViewCells;
using Exchange.Configs;
using Exchange.Controls;
using Xamarin.Forms;

namespace Exchange.Pages
{
	public partial class AskDetailPage
	{
		private StackLayout _mainLayout;

		private StackLayout _questionLayout;
		private Label _titleLabel;
		private Label _subtitleLabel;
		private Label _descriptionLabel;
		private Label _responsesLabel;

		private StackLayout _repliesLayout;
		private ListView _repliesListview;

		protected override void InitializeComponents()
		{
			Title = "Ask";
			BackgroundColor = Color.White;

			_mainLayout = new StackLayout();

			var scrollContent = new StackLayout
			{
				VerticalOptions = LayoutOptions.FillAndExpand,
			};
			var scrollLayout = new ScrollView
			{
				Content = scrollContent,
			};
			_mainLayout.Children.Add(scrollLayout);


			#region question

			_questionLayout = new StackLayout
			{
				Padding = new Thickness(20),
			};
			scrollContent.Children.Add(_questionLayout);

			_titleLabel = new Label
			{
				Style = Styles.Title,
			};
			_questionLayout.Children.Add(_titleLabel);

			_subtitleLabel = new Label
			{
				Style = Styles.Subtitle,
			};
			_questionLayout.Children.Add(_subtitleLabel);

			_descriptionLabel = new Label
			{
				Style = Styles.Verbosa,
			};
			_questionLayout.Children.Add(_descriptionLabel);

			_responsesLabel = new Label
			{
				Style = Styles.Subtitle,
				TextColor = Styles.Colors.NormalText,
			};
			_questionLayout.Children.Add(_responsesLabel);

			#endregion

			#region Replies

			_repliesLayout = new StackLayout();
			_repliesListview = new ListView
			{
				HasUnevenRows = true,
				SeparatorVisibility = SeparatorVisibility.None,
				ItemTemplate = new DataTemplate(typeof(CommentViewCell)),
			};
			_repliesListview.ItemSelected += RepliesListview_ItemSelected;
			_repliesLayout.Children.Add(_repliesListview);
			scrollContent.Children.Add(_repliesLayout);

			#endregion

			#region ToolbarItems

			var shareToolbarItem = new ToolbarItem
			{
				Text = "Compartir",
				Icon="ic_share.png"
			};
			shareToolbarItem.Clicked += ShareToolbarItem_Clicked;
			ToolbarItems.Add(shareToolbarItem);

			var replyToolbarItem = new ToolbarItem
			{
				Text = "Responder",
				Icon = "ic_ask_white.png"
			};
			replyToolbarItem.Clicked += ReplyToolbarItem_Clicked;
			ToolbarItems.Add(replyToolbarItem);

			#endregion
		}
	}
}


