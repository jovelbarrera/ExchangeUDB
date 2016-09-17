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
    public partial class AskPage : ContentPage
    {
        private ObservableCollection<Ask> _askListViewItemsSource;
        public AskPage()
        {
            _askListViewItemsSource = new ObservableCollection<Ask>();
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
            List<Ask> askList = new List<Ask>();
            askList = await AskService.Instance.GetLatest();

            if (askList == null)
            {
                Content = new EmptyContent();
                return;
            }

            askList = askList.OrderByDescending(i => i.CreatedAt).ToList();

            _askListViewItemsSource.Clear();

            foreach (var ask in askList)
            {
                if (ask.User != null)
                {
                    GenericResponse<User> response = await FirebaseUserService.Instance.Read(ask.User.ObjectId);
                    if (response != null)
                        ask.User = response.Model;
                }
                _askListViewItemsSource.Add(ask);
            }

            //if (_askListViewItemsSource != null && _askListViewItemsSource.Count > 0)
            Content = _mainLayout;
            //else
            //    Content = new EmptyContent();
        }

        private void AskList_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem == null)
                return;

            var list = (ListView)sender;
            var ask = (Ask)e.SelectedItem;
            Navigation.PushAsync(new AskDetailPage(ask));
            list.SelectedItem = null;
        }

        private async void LoadMoreButton_Clicked(object sender, EventArgs e)
        {
            await LoadNext();
        }

        private async Task LoadNext()
        {
            List<Ask> askList = await AskService.Instance.GetPrev(10, _askListViewItemsSource.LastOrDefault().ObjectId);
            askList = askList.OrderByDescending(i => i.CreatedAt).ToList();

            foreach (var ask in askList)
            {
                GenericResponse<User> response = await FirebaseUserService.Instance.Read(ask.User.ObjectId);
                if (response != null)
                    ask.User = response.Model;
            }
            foreach (var ask in askList)
            {
                if (!_askListViewItemsSource.Any(i => i.ObjectId == ask.ObjectId))
                    _askListViewItemsSource.Add(ask);
            }

            //if (_askListViewItemsSource != null && _askListViewItemsSource.Count > 0)
            //	Content = _mainLayout;
            //else
            //	Content = new EmptyContent();

        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            if (Device.OS == TargetPlatform.Android)
                DependencyService.Get<IToolsService>().TabLayout();
        }
    }
}


