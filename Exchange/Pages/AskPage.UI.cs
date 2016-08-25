using System;
using Exchange.Configs;
using Exchange.ViewCells;
using Xamarin.Forms;

namespace Exchange.Pages
{
	public partial class AskPage : ContentPage
	{
		private StackLayout _mainLayout;
		private ListView _askListView;

		public void InitializeComponents()
		{
			Title = "Ask";
			BackgroundColor = Color.White;

			_mainLayout = new StackLayout
			{
				VerticalOptions = LayoutOptions.FillAndExpand,
			};

			_askListView = new ListView
			{
				ItemTemplate = new DataTemplate(typeof(AskViewCell)),
				HasUnevenRows = true,
				SeparatorVisibility = SeparatorVisibility.None,
				VerticalOptions = LayoutOptions.FillAndExpand,
			};
			_askListView.ItemSelected += AskList_ItemSelected;
			_mainLayout.Children.Add(_askListView);

			var loadMoreButton = new Button
			{
				Style = Styles.ActiveButton,
				Text = "Cargar más",
				VerticalOptions = LayoutOptions.End
			};
			loadMoreButton.Clicked += LoadMoreButton_Clicked;
			_askListView.Footer = loadMoreButton;
		}
	}
}


