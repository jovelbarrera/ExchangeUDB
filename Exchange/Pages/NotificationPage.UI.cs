using System;
using Exchange.ViewCells;
using Xamarin.Forms;

namespace Exchange.Pages
{
	public partial class NotificationPage : ContentPage
	{
		private StackLayout _mainLayout;
		private ListView _notificationsListview;

		public void InitializeComponents()
		{
			Title = "Notificaciones";
			BackgroundColor = Color.White;

			_mainLayout = new StackLayout();

			_notificationsListview = new ListView
			{
				HasUnevenRows = true,
				ItemTemplate = new DataTemplate(typeof(NotificationViewCell)),
				SeparatorVisibility = SeparatorVisibility.None,
			};
			_notificationsListview.ItemSelected += NotificationsListview_ItemSelected;
			_mainLayout.Children.Add(_notificationsListview);

			//Content = mainLayout;
		}
	}
}


