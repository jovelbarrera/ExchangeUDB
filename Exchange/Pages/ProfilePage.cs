using System.Threading.Tasks;
using Exchange.ContentViews;
using Exchange.Models;
using Plugin.Connectivity;
using Xamarin.Forms;

namespace Exchange.Pages
{
	public partial class ProfilePage : ContentPage
	{
		private User _user;

		public ProfilePage(User user)
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
				_pointsLabel.Text = "100";
				_firstnameLabel.Text = _user.FirstName;
                _lastnameLabel.Text = _user.LastName;
                _emailLabel.Text = _user.Email;
				_universityLabel.Text = (string)_user.GetData("University");
				_careerLabel.Text = (string)_user.GetData("Career");
				_aboutMeLabel.Text = (string)_user.GetData("About");

				Content = _mainLayout;
			}
			else
			{
				Content = new EmptyContent();
			}
		}
	}
}

