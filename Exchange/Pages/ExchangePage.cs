using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Exchange.Configs;
using Exchange.ContentViews;
using Exchange.Dependencies;
using Exchange.Models;
using Plugin.Connectivity;
using Xamarin.Forms;
using Exchange.Services;

namespace Exchange.Pages
{
	public partial class ExchangePage : ContentPage
	{
		public ExchangePage()
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
            List<Models.Video> itemSource = await VideoService.Instance.GetLatest();//Dummy.ExchangeList();
			_exchangeListView.ItemsSource = itemSource;
			if (itemSource != null && itemSource.Count > 0)
				Content = _mainLayout;
			else
				Content = new EmptyContent();
		}

		private void ExchangeListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
		{
			if (e.SelectedItem == null)
				return;

			var list = (ListView)sender;
			var exchange = (Exchange.Models.Video)e.SelectedItem;
			Navigation.PushAsync(new ExchangeDetailPage(exchange));
			list.SelectedItem = null;
		}

		protected override void OnAppearing()
		{
			base.OnAppearing();
			if (Device.OS == TargetPlatform.Android)
				DependencyService.Get<IToolsService>().TabLayout();
		}
	}
}


