using System;
using Exchange.Configs;
using Exchange.Controls;
using Exchange.ViewCells;
using Xamarin.Forms;

namespace Exchange.Pages
{
	public partial class SearchPage : ContentPage
	{
		private StackLayout _mainLayout;
		private CustomSearchBar _searchBar;
		private Label _resultsLabel;
		private ListView _resultsListview;

		public void InitializeComponents()
		{
			Title = "Búsqueda";
			BackgroundColor = Color.White;

			var scrollLayout = new StackLayout();

			var searchLayout = new StackLayout();
			_searchBar = new CustomSearchBar
			{
				BackgroundColor = Styles.Colors.Primary,
				CancelButtonColor = Color.White,
				PlaceholderColor = Color.White,
				TextColor = Color.White,
				Placeholder = "Buscar",
			};
			searchLayout.Children.Add(new StackLayout
			{
				Padding = new Thickness(0, 10, 10, 10),
				BackgroundColor = Styles.Colors.Primary,
				Children = { _searchBar }
			});

			_resultsLabel = new Label
			{
				Style = Styles.Subtitle,
				TextColor = Styles.Colors.NormalText,
			};
			//scrollLayout.Children.Add(
			var resultHeader = new StackLayout
			{
				Padding = new Thickness(20, 10),
				Children = { _resultsLabel }
			};

			_resultsListview = new ListView
			{
				Header = resultHeader,
				VerticalOptions = LayoutOptions.FillAndExpand,
				HasUnevenRows = true,
				SeparatorVisibility = SeparatorVisibility.None,
				ItemTemplate = new ResultsTemplateSelector(),
			};
			_resultsListview.ItemSelected += ResultsListview_ItemSelected;
			scrollLayout.Children.Add(_resultsListview);

			_mainLayout = new StackLayout
			{
				Spacing = 0,
				Children ={
					searchLayout,
					new ScrollView {
						VerticalOptions = LayoutOptions.FillAndExpand,
						Content = scrollLayout,
					},
				},
			};
		}
	}
}