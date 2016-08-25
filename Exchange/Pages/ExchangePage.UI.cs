using System;
using Exchange.ViewCells;
using Xamarin.Forms;

namespace Exchange.Pages
{
	public partial class ExchangePage : ContentPage
	{
		private StackLayout _mainLayout;
		private ListView _exchangeListView;

		public void InitializeComponents()
		{
			Title = "Exchanges";
			BackgroundColor = Color.White;

			_mainLayout = new StackLayout
			{
			};

			_exchangeListView = new ListView
			{
				ItemTemplate = new DataTemplate(typeof(ExchangeViewCell)),
				HasUnevenRows = true,
				SeparatorVisibility = SeparatorVisibility.None,
			};
			_exchangeListView.ItemSelected+= ExchangeListView_ItemSelected;
			_mainLayout.Children.Add(_exchangeListView);

			//Content = _mainLayout;
		}
	}
}


