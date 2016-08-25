using System;
using Exchange.Configs;
using Exchange.Controls;
using Exchange.Models;
using Xamarin.Forms;

namespace Exchange.ViewCells
{
	public class NotificationViewCell : ViewCell
	{
		private CustomImage _pictureImage;
		private Label _headerLabel;
		private Label _subheaderLabel;
		private Label _contentLabel;

		public NotificationViewCell()
		{
			var maiLayout = new StackLayout
			{
				Padding = new Thickness(20, 0),
				Spacing = 20,
			};

			var containerLayout = new StackLayout
			{
				Orientation = StackOrientation.Horizontal,
				Spacing = 15,
				Padding = new Thickness(10, 20, 10, 0),
			};
			maiLayout.Children.Add(containerLayout);

			#region Picture
			var imageLayout = new StackLayout();
			containerLayout.Children.Add(imageLayout);
			_pictureImage = new CustomImage
			{
				Aspect = Aspect.AspectFill,
				Source = "ic_picture_placeholder.png",
				HeightRequest = 60,
			};
			imageLayout.Children.Add(_pictureImage);
			#endregion

			#region Content
			var contentLayout = new StackLayout
			{
				Spacing = 0,
			};
			containerLayout.Children.Add(contentLayout);
			_headerLabel = new Label
			{
				Style = Styles.Title,
			};
			contentLayout.Children.Add(_headerLabel);

			_contentLabel = new Label
			{
				Style = Styles.Verbosa,
			};
			contentLayout.Children.Add(_contentLabel);

			_subheaderLabel = new Label
			{
				Style = Styles.Subtitle,
				Text = "",
			};
			contentLayout.Children.Add(_subheaderLabel);
			#endregion


			maiLayout.Children.Add(UIHelper.UIHelper.Separator());

			View = maiLayout;
		}

		protected override void OnBindingContextChanged()
		{
			base.OnBindingContextChanged();

			Notification notification = this.BindingContext as Notification;

			if (notification == null)
				return;

			if (notification.User != null)
			{
				if (!string.IsNullOrEmpty(notification.User.ProfilePicture))
					_pictureImage.Source = notification.User.ProfilePicture;
				if (!string.IsNullOrEmpty(notification.User.DisplayName))
					_headerLabel.Text = notification.User.DisplayName;
			}
			_subheaderLabel.Text = notification.Time.ToString();
			_contentLabel.Text = notification.Detail;
			//_contentLabel.Text = Tools.TruncateText(comment.message, 100);

		}
	}
}


