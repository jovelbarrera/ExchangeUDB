using System;
using System.Threading.Tasks;
using Exchange.Configs;
using Xamarin.Forms;

namespace Exchange.Pages
{
	public class MainPage : MasterDetailPage
	{
		public static MainPage Instance;

		public MainPage()
		{
			Instance = this;
			var masterPage = new MenuPage();
			masterPage.Title = "Menu";
			Master = masterPage;
			Detail = Tools.CreateNavigationPage(new HomePage());
		}

		public async Task SetAsRootPage(Page page)
		{
			Detail = Tools.CreateNavigationPage(page);
		}

		public async Task PushPage(Page page)
		{
			Detail.Navigation.PushAsync(page);
		}

		public void PopPage()
		{
			Detail.Navigation.PopAsync();
		}
	}
}

