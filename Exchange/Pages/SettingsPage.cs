using System;
using System.Threading.Tasks;
using Exchange.Configs;
using Exchange.Interfaces;
using Exchange.Models;
using Exchange.Services;
using Xamarin.Forms;

namespace Exchange.Pages
{
	public class SettingsPage : TabbedPage
	{
		public SettingsPage()
		{
			Title = "Configuración";
			LoadSettings().ConfigureAwait(false);
		}

		private async Task LoadSettings()
		{
			IUser currentUser = await CustomUserManager.Instance.GetCurrentUser();
			Children.Add(new ProfilePage(new User(currentUser)));
			Children.Add(new ActivityPage());
			Children.Add(new EditProfilePage(new User(currentUser)));
		}
	}
}

