using System.Collections.Generic;
using System.Threading.Tasks;
using Exchange.Configs;
using Exchange.ContentViews;
using Exchange.Interfaces;
using Exchange.Models;
using Plugin.Connectivity;
using Xamarin.Forms;

namespace Exchange.Pages
{
	public partial class SearchPage : ContentPage
	{
		public SearchPage()
		{
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
			Content = new LoadingContent();
			List<Models.Exchange> exchangesResults = await Dummy.ExchangeList();
			List<Ask> askResults = await Dummy.AskList();
			int resultNumber = exchangesResults.Count + askResults.Count;
			if (exchangesResults == null || askResults == null || resultNumber <= 0)
			{
				Content = new EmptyContent();
				return;
			}
			_resultsLabel.Text = resultNumber.ToString() + " Resultados";
			var results = new List<IModel>();
			foreach (var exchangesResult in exchangesResults)
				results.Add(exchangesResult);
			foreach (var askResult in askResults)
				results.Add(askResult);
			_resultsListview.ItemsSource = results;
			Content = _mainLayout;
		}

		private void ResultsListview_ItemSelected(object sender, SelectedItemChangedEventArgs e)
		{

		}
	}
}


