using System.Threading.Tasks;
using Exchange.ContentViews;
using Exchange.Models;
using Plugin.Connectivity;
using Xamarin.Forms;

namespace Exchange.Pages
{
	public partial class EditProfilePage : ContentPage
	{
		private User _user;

		public EditProfilePage(User user)
		{
			_user = user;
			Content = new LoadingContent();
			InitializeComponents();
			ConnectionManager();
		}

		private void ConnectionManager()
		{
			if (!CrossConnectivity.Current.IsConnected)
				Content = new ConnectionFailContent((s, e) => ConnectionManager());
			else
				LoadData().ConfigureAwait(false);
		}

		private async Task LoadData()
		{
			if (_user != null)
			{
				if (!string.IsNullOrEmpty(_user.ProfilePicture))
					_pictureImage.Source = _user.ProfilePicture;
				_nameLabel.Text = _user.DisplayName;
				_emailLabel.Text = _user.Email;
				_universityLabel.Text = _user.University;
				_careerLabel.Text = _user.Career;
				_aboutMeLabel.Text = _user.About;

				Content = _mainLayout;
			}
			else
			{
				Content = new EmptyContent();
			}
		}
	}
}

