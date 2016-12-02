using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Exchange.Configs;
using Exchange.ContentViews;
using Exchange.Dependencies;
using Exchange.Models;
using Exchange.Services;
using Exchange.Services.FirebaseServices;
using Kadevjo.Core.Models;
using Plugin.Connectivity;
using Xamarin.Forms;

namespace Exchange.Pages
{
	public partial class QuestionPage : ContentPage
	{
		private ObservableCollection<Question> _askListViewItemsSource;
		public QuestionPage()
		{
			_askListViewItemsSource = new ObservableCollection<Question>();
			Content = new LoadingContent();
			InitializeComponents();
			_askListView.ItemsSource = _askListViewItemsSource;
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
			List<Question> askList = new List<Question>();
			askList = await QuestionService.Instance.GetLatest();

			if (askList == null)
			{
				Content = new EmptyContent();
				return;
			}

			askList = askList.OrderByDescending(i => i.CreatedAt).ToList();

			_askListViewItemsSource.Clear();

			foreach (var ask in askList)
			{
				if (!string.IsNullOrEmpty(ask.UserId))
				{
					ask.User = await UserService.Instance.GetUser(ask.UserId);
				}
				_askListViewItemsSource.Add(ask);
			}

			Content = _mainLayout;
		}

		private void QuestionList_ItemSelected(object sender, SelectedItemChangedEventArgs e)
		{
			if (e.SelectedItem == null)
				return;

			var list = (ListView)sender;
			var ask = (Question)e.SelectedItem;
			Navigation.PushAsync(new QuestionDetailPage(ask));
			list.SelectedItem = null;
		}

		private async void DoneToolbarItem_Clicked(object sender, EventArgs e)
		{
			await Navigation.PushAsync(new EditQuestion());
		}
	}
}


