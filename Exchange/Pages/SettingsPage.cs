using System;
using Exchange.Configs;
using Exchange.Models;
using Xamarin.Forms;

namespace Exchange.Pages
{
	public class SettingsPage : TabbedPage
	{
		public SettingsPage()
		{
			Title = "Configuración";

			User currentUser = Dummy.User();
			Children.Add(new ProfilePage(currentUser));
			Children.Add(new ActivityPage());
			Children.Add(new EditProfilePage(currentUser));
		}
	}
}

