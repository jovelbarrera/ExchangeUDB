using System;
using Exchange.Configs;
using Exchange.Controls;
using Xamarin.Forms;

namespace Exchange.Pages
{
	public partial class EditProfilePage
	{
		private ScrollView _mainLayout;
		private CustomImage _pictureImage;
		private CustomEntry _nameLabel;
		private CustomEntry _emailLabel;
		private CustomEntry _universityLabel;
		private CustomEntry _careerLabel;
		private CustomEditor _aboutMeLabel;
		private Label _charactersLeftLabel;

		public void InitializeComponents()
		{
			Title = "Editar Perfil";
			BackgroundColor = Color.White;
			var scrollLayout = new StackLayout();

			var profileLayout = new StackLayout
			{
				Padding = new Thickness(20),
				Spacing = 0
			};
			scrollLayout.Children.Add(profileLayout);

			#region top
			var topLayout = new RelativeLayout();
			profileLayout.Children.Add(topLayout);
			_pictureImage = new CustomImage
			{
				Aspect = Aspect.AspectFill,
				Source = "ic_picture_placeholder.png",
				WidthRequest = 100,
				VerticalOptions = LayoutOptions.Center,
				HorizontalOptions = LayoutOptions.Center,
			};
			topLayout.Children.Add(_pictureImage,
				Constraint.RelativeToParent(p => p.Width / 2 - 50),
				Constraint.Constant(0),
				Constraint.Constant(100),
				Constraint.Constant(100)
			);
			var cameraImage = new Image
			{
				Source = "ic_photo.png",
				HeightRequest = 40,
				VerticalOptions = LayoutOptions.Center,
				HorizontalOptions = LayoutOptions.Center,
			};
			topLayout.Children.Add(cameraImage,
				Constraint.RelativeToParent(p => p.Width / 2 - 50),
				Constraint.Constant(0),
				Constraint.Constant(100),
				Constraint.Constant(100)
			);
			#endregion

			#region Info
			var infoLayout = new StackLayout
			{
				Padding = new Thickness(20),
				Spacing = 5,
			};
			scrollLayout.Children.Add(infoLayout);

			_nameLabel = new CustomEntry
			{
				Style = Styles.NormalEntry,
				Placeholder = "Nombre",
			};
			infoLayout.Children.Add(UIHelper.UIHelper.FormGroup("ic_user.png", _nameLabel));

			_emailLabel = new CustomEntry
			{
				Style = Styles.NormalEntry,
				Placeholder = "Email",
			};
			infoLayout.Children.Add(UIHelper.UIHelper.FormGroup("ic_mail.png", _emailLabel));

			_universityLabel = new CustomEntry
			{
				Style = Styles.NormalEntry,
				Placeholder = "Universidad",
			};
			infoLayout.Children.Add(UIHelper.UIHelper.FormGroup("ic_university.png", _universityLabel));

			_careerLabel = new CustomEntry
			{
				Style = Styles.NormalEntry,
				Placeholder = "Carrera",
			};
			infoLayout.Children.Add(UIHelper.UIHelper.FormGroup(string.Empty, _careerLabel));

			_aboutMeLabel = new CustomEditor
			{
				Style = Styles.NormalEditor,
				Text = "Sobre mi",
				HeightRequest = 100,
			};
			infoLayout.Children.Add(UIHelper.UIHelper.FormGroup(string.Empty, _aboutMeLabel));
			_charactersLeftLabel = new Label
			{
				Text = "100@",
				HorizontalTextAlignment = TextAlignment.End,
			};
			infoLayout.Children.Add(_charactersLeftLabel);
			#endregion

			_mainLayout = new ScrollView
			{
				Content = scrollLayout,
			};
		}
	}
}

