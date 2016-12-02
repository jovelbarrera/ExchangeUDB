using System;
using Exchange.Configs;
using Exchange.ViewCells;
using Xamarin.Forms;

namespace Exchange.Pages
{
	public partial class QuestionPage : ContentPage
	{
		private StackLayout _mainLayout;
		private ListView _askListView;

		public void InitializeComponents()
		{
			Title = "Preguntas";
			BackgroundColor = Color.White;

			_mainLayout = new StackLayout
			{
				VerticalOptions = LayoutOptions.FillAndExpand,
			};

			_askListView = new ListView
			{
				ItemTemplate = new DataTemplate(typeof(QuestionViewCell)),
				HasUnevenRows = true,
				SeparatorVisibility = SeparatorVisibility.None,
				VerticalOptions = LayoutOptions.FillAndExpand,
			};
			_askListView.ItemSelected += QuestionList_ItemSelected;
			_mainLayout.Children.Add(_askListView);

			var newToolbarItem = new ToolbarItem
			{
				Text = "Nueva pregunta",
			};

			if (Device.OS == TargetPlatform.Android)
				newToolbarItem.Icon = "ic_action_done.png";

			newToolbarItem.Clicked += DoneToolbarItem_Clicked;
			ToolbarItems.Add(newToolbarItem);
		}
	}
}


