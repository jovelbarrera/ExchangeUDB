using System;
using System.Threading.Tasks;
using Exchange.Configs;
using Exchange.Interfaces;
using Exchange.Models;
using Exchange.Services;
using Xamarin.Forms;

namespace Exchange.Pages
{
	public partial class MenuPage : ContentPage
	{
		public MenuItem CurrentActive;

		public MenuPage()
		{
			CurrentActive = MenuItem.Home;
			Initializecomponents();
			LoadData().ConfigureAwait(false);
		}

		private async Task LoadData()
		{
			IUser user = UserManager.Instance.CurrentUser;
			if (user != null)
			{
				if (!string.IsNullOrEmpty(user.ProfilePicture))
					_pictureImage.Source = user.ProfilePicture;
				_nameLabel.Text = user.DisplayName;
				_universityLabel.Text = user.University;
				_pointsLabel.Text = "100";
			}
		}

		private void SelectMenuItem(object sender, MenuItem menuItem)
		{
			MainPage.Instance.IsPresented = false;
			if (MainPage.Instance == null ||
				sender == null ||
				CurrentActive == menuItem)
				return;

			CurrentActive = menuItem;
			var view = (View)sender;
			UnfocusMenuItem();
			switch (menuItem)
			{
				case MenuItem.Notification:
					MainPage.Instance.SetAsRootPage(new NotificationPage());
					break;
				case MenuItem.Home:
					MainPage.Instance.SetAsRootPage(new HomePage());
					break;
				case MenuItem.Ask:
					MainPage.Instance.SetAsRootPage(new EditAsk());
					break;
				case MenuItem.Exchange:
					//MainPage.Instance.PushPage(new NewAsk());
					break;
				case MenuItem.Extra:
					//MainPage.Instance.PushPage(new NewAsk());
					break;
				case MenuItem.Search:
					MainPage.Instance.SetAsRootPage(new SearchPage());
					break;
				case MenuItem.Settings:
					MainPage.Instance.SetAsRootPage(new SettingsPage());
					break;
				case MenuItem.Help:
					//MainPage.Instance.PushPage(new NewAsk());
					break;
				case MenuItem.About:
					//MainPage.Instance.PushPage(new NewAsk());
					break;
			}
			view.BackgroundColor = Styles.Colors.Placeholder;
		}

		private void UnfocusMenuItem()
		{
			_notification.BackgroundColor = Color.White;
			_home.BackgroundColor = Color.White;
			_ask.BackgroundColor = Color.White;
			_exchanges.BackgroundColor = Color.White;
			_extras.BackgroundColor = Color.White;
			_search.BackgroundColor = Color.White;
			_settings.BackgroundColor = Color.White;
			_help.BackgroundColor = Color.White;
			_about.BackgroundColor = Color.White;
		}
	}
}


