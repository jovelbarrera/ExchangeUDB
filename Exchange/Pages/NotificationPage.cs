using System;
using System.Threading.Tasks;
using Exchange.Configs;
using Exchange.ContentViews;
using Exchange.Models;
using Plugin.Connectivity;
using Xamarin.Forms;

namespace Exchange.Pages
{
	public partial class NotificationPage : ContentPage
	{
		public NotificationPage()
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
			_notificationsListview.ItemsSource = await Dummy.NotificationList();
			Content = _mainLayout;
		}

		private void NotificationsListview_ItemSelected(object sender, SelectedItemChangedEventArgs e)
		{
			if (e.SelectedItem == null)
				return;

			var list = (ListView)sender;
			var notification = (Notification)e.SelectedItem;
			switch (notification.Type)
			{
				case NotificationType.AskActivity:
					Navigation.PushAsync(new AskDetailPage(notification.ResourceId));
					break;
				case NotificationType.NewExchange:
					Navigation.PushAsync(new ExchangeDetailPage(notification.ResourceId));
					break;
				case NotificationType.Feed:
					//Navigation.PushAsync(new ExtraDetailPage(notification.ResourceId);
					break;
			}
			list.SelectedItem = null;
		}
	}
}


