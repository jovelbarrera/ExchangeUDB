using System;
using Exchange.Configs;
using Exchange.Controls;
using Exchange.Models;
using Xamarin.Forms;

namespace Exchange.ViewCells
{
	public class ExchangeViewCell : ViewCell
	{
		private Image _thumbnailImage;
		private Label _headerLabel;
		private Label _descriptionLabel;
		private Label _likesLabel;

		public ExchangeViewCell()
		{
			var maiLayout = new RelativeLayout
			{
				HeightRequest = 200,
			};

			#region Background
			_thumbnailImage = new Image
			{
				Aspect = Aspect.AspectFill,
			};
			maiLayout.Children.Add(_thumbnailImage,
				Constraint.Constant(0),
				Constraint.Constant(0),
				Constraint.RelativeToParent(p => p.Width),
				Constraint.RelativeToParent(p => p.Height)
			);
			maiLayout.Children.Add(new BoxView { BackgroundColor = Color.Black, Opacity = 0.4 },
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
			maiLayout.Children.Add(playImage,
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

			_descriptionLabel = new Label
			{
				Style = Styles.Verbosa,
				TextColor = Color.White,
			};
			videoDescriptionLayout.Children.Add(_descriptionLabel);

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
			maiLayout.Children.Add(container,
				Constraint.Constant(20),
				Constraint.RelativeToParent(p => p.Height - 100),
				Constraint.RelativeToParent(p => p.Width * 0.8),
				Constraint.Constant(100)
			);
			#endregion

			View = maiLayout;
		}

		protected override void OnBindingContextChanged()
		{
			base.OnBindingContextChanged();

			Exchange.Models.Exchange exchange = this.BindingContext as Exchange.Models.Exchange;

			if (exchange == null)
				return;

			_thumbnailImage.Source = exchange.Thumbnail;
			_headerLabel.Text = exchange.Title;
			_descriptionLabel.Text = Tools.TruncateText(exchange.Description, 50);

		}
	}
}


