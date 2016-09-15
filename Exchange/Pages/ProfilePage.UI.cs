using System;
using Exchange.Configs;
using Exchange.Controls;
using Xamarin.Forms;

namespace Exchange.Pages
{
	public partial class ProfilePage
	{
		private StackLayout _mainLayout;
		private CustomImage _pictureImage;
		private Label _pointsLabel;
		private Label _nameLabel;
		private Label _emailLabel;
		private Label _universityLabel;
		private Label _careerLabel;
		private Label _aboutMeLabel;

		public void InitializeComponents()
		{
			Title = "Perfil";
			BackgroundColor = Color.White;
			_mainLayout = new StackLayout();

			var profileLayout = new StackLayout
			{
				Padding = new Thickness(20, 30, 20, 20),
			};
			_mainLayout.Children.Add(profileLayout);

			#region top
			var topLayout = new StackLayout
			{
				Orientation = StackOrientation.Horizontal,
			};
			profileLayout.Children.Add(topLayout);
			_pictureImage = new CustomImage
			{
				Aspect = Aspect.AspectFill,
				Source = "ic_picture_placeholder.png",
				WidthRequest = 80,
				HeightRequest = 80,
				VerticalOptions = LayoutOptions.Start,
				HorizontalOptions = LayoutOptions.Start,
			};
			topLayout.Children.Add(_pictureImage);
			var starImage = new Image
			{
				Source = "ic_circle_star.png",
				HeightRequest = 30,
				VerticalOptions = LayoutOptions.Start,
				HorizontalOptions = LayoutOptions.EndAndExpand,
			};
			topLayout.Children.Add(starImage);
			_pointsLabel = new Label
			{
				Style = Styles.Title,
				TextColor = Styles.Colors.NormalText,
				VerticalOptions = LayoutOptions.Start,
				HorizontalOptions = LayoutOptions.End,
			};
			topLayout.Children.Add(_pointsLabel);
			#endregion

			#region Info
			var infoLayout = new StackLayout
			{
				Padding = new Thickness(20),
				Spacing = 15,
			};
			_mainLayout.Children.Add(infoLayout);

			_nameLabel = new Label
			{
				Style = Styles.Verbosa,
			};
			infoLayout.Children.Add(UIHelper.UIHelper.FormGroup("ic_user.png", _nameLabel));

			_emailLabel = new Label
			{
				Style = Styles.Verbosa,
			};
			infoLayout.Children.Add(UIHelper.UIHelper.FormGroup("ic_mail.png", _emailLabel));

			_universityLabel = new Label
			{
				Style = Styles.Verbosa,
			};
			infoLayout.Children.Add(UIHelper.UIHelper.FormGroup("ic_university.png", _universityLabel));

			_careerLabel = new Label
			{
				Style = Styles.Verbosa,
			};
			infoLayout.Children.Add(UIHelper.UIHelper.FormGroup(string.Empty, _careerLabel));

			_aboutMeLabel = new Label
			{
				Style = Styles.Verbosa,
			};
			infoLayout.Children.Add(UIHelper.UIHelper.FormGroup(string.Empty, _aboutMeLabel));
			#endregion
		}
	}
}

