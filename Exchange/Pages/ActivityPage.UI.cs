using System;
using Exchange.ViewCells;
using Xamarin.Forms;

namespace Exchange.Pages
{
	public partial class ActivityPage
	{
		private StackLayout _mainLayout;
		private ListView _activityListview;

		public void InitializeComponents()
		{
			Title = "Actividad";
			BackgroundColor = Color.White;

			_mainLayout = new StackLayout();

			_activityListview = new ListView
			{
				HasUnevenRows = true,
				ItemTemplate = new DataTemplate(typeof(ActivityViewCell)),
				SeparatorVisibility = SeparatorVisibility.None,
			};
			_activityListview.ItemSelected += ActivityListview_ItemSelected;
			_mainLayout.Children.Add(_activityListview);
		}
	}
}

