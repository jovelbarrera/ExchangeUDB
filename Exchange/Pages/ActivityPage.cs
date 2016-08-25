using System;
using System.Threading.Tasks;
using Exchange.Configs;
using Exchange.ContentViews;
using Plugin.Connectivity;
using Xamarin.Forms;

namespace Exchange.Pages
{
	public partial class ActivityPage : ContentPage
	{
		public ActivityPage()
		{
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
			_activityListview.ItemsSource = await Dummy.ActivityList();
			Content = _mainLayout;
		}

		private void ActivityListview_ItemSelected(object sender, SelectedItemChangedEventArgs e)
		{
			if (e.SelectedItem == null)
				return;

			var list = (ListView)sender;

			list.SelectedItem = null;
		}
	}
}

