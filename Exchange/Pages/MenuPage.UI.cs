using System;
using Exchange.Configs;
using Exchange.Controls;
using Xamarin.Forms;

namespace Exchange.Pages
{
	public enum MenuItem
	{
		Notification,
		Home,
		Ask,
		Exchange,
		Extra,
		Search,
		Settings,
		Help,
		About
	}
	public partial class MenuPage
	{
		private CustomImage _pictureImage;
		private Label _nameLabel;
		private Label _universityLabel;
		private Label _pointsLabel;
		private StackLayout _notification;
		private StackLayout _home;
		private StackLayout _ask;
		private StackLayout _exchanges;
		private StackLayout _extras;
		private StackLayout _search;
		private StackLayout _settings;
		private StackLayout _help;
		private StackLayout _about;

		public void Initializecomponents()
		{
			BackgroundColor = Color.White;
			var mainLayout = new StackLayout();

			var profileLayout = new StackLayout
			{
				BackgroundColor = Styles.Colors.Primary,
				Padding = new Thickness(20, 30, 20, 20),
			};
			mainLayout.Children.Add(profileLayout);

			#region top
			var topLayout = new StackLayout
			{
				Orientation = StackOrientation.Horizontal,
			};
			profileLayout.Children.Add(topLayout);
			_pictureImage = new CustomImage
			{
				Aspect = Aspect.Fill,
				Source = "ic_picture_placeholder.png",
				WidthRequest = 60,
				HeightRequest = 60,
				VerticalOptions = LayoutOptions.Start,
				HorizontalOptions = LayoutOptions.Start,
			};
			topLayout.Children.Add(_pictureImage);
			var starImage = new Image
			{
				Source = "ic_circle_star.png",
				HeightRequest = 30,
				VerticalOptions = LayoutOptions.Start,
				HorizontalOptions = LayoutOptions.EndAndExpand,
			};
			topLayout.Children.Add(starImage);
			_pointsLabel = new Label
			{
				Style = Styles.Title,
				TextColor = Color.White,
				VerticalOptions = LayoutOptions.Start,
				HorizontalOptions = LayoutOptions.End,
			};
			topLayout.Children.Add(_pointsLabel);
			#endregion

			#region info
			var infoLayout = new StackLayout();
			profileLayout.Children.Add(infoLayout);
			_nameLabel = new Label
			{
				Style = Styles.Title,
				TextColor = Color.White,
				LineBreakMode = LineBreakMode.TailTruncation,
			};
			infoLayout.Children.Add(_nameLabel);
			_universityLabel = new Label
			{
				Style = Styles.Verbosa,
				TextColor = Color.White,
				LineBreakMode = LineBreakMode.TailTruncation,
			};
			infoLayout.Children.Add(_universityLabel);
			#endregion

			#region menu
			var menuLayout = new StackLayout
			{
				Padding = new Thickness(0, 20),
			};

			_notification = MenuItemLayout("ic_notification_red.png", "Notificaciones");
			_notification.AddTapHandler((s, e) => SelectMenuItem(s, MenuItem.Notification));
			menuLayout.Children.Add(_notification);
			_home = MenuItemLayout("ic_home_blue.png", "Inicio");
			_home.AddTapHandler((s, e) => SelectMenuItem(s, MenuItem.Home));
			_home.BackgroundColor = Styles.Colors.Placeholder;
			menuLayout.Children.Add(_home);
			_ask = MenuItemLayout("ic_ask_yellow_menu.png", "Ask");
			_ask.AddTapHandler((s, e) => SelectMenuItem(s, MenuItem.Ask));
			menuLayout.Children.Add(_ask);
			_exchanges = MenuItemLayout("ic_exchange_red.png", "Exchanges");
			_exchanges.AddTapHandler((s, e) => SelectMenuItem(s, MenuItem.Exchange));
			menuLayout.Children.Add(_exchanges);
			_extras = MenuItemLayout("ic_plus.png", "Extra");
			_extras.AddTapHandler((s, e) => SelectMenuItem(s, MenuItem.Extra));
			menuLayout.Children.Add(_extras);
			_search = MenuItemLayout("ic_search.png", "Búsqueda");
			_search.AddTapHandler((s, e) => SelectMenuItem(s, MenuItem.Search));
			menuLayout.Children.Add(_search);

			menuLayout.Children.Add(UIHelper.UIHelper.Separator());

			_settings = MenuItemLayout("ic_settings.png", "Configuración");
			_settings.AddTapHandler((s, e) => SelectMenuItem(s, MenuItem.Settings));
			menuLayout.Children.Add(_settings);

			menuLayout.Children.Add(UIHelper.UIHelper.Separator());

			_help = MenuItemLayout(string.Empty, "Ayuda y Feedback");
			_help.AddTapHandler((s, e) => SelectMenuItem(s, MenuItem.Help));
			menuLayout.Children.Add(_help);
			_about = MenuItemLayout(string.Empty, "Acerca de Exchange");
			_about.AddTapHandler((s, e) => SelectMenuItem(s, MenuItem.About));
			menuLayout.Children.Add(_about);

			mainLayout.Children.Add(menuLayout);
			#endregion

			Content = new ScrollView { Content = mainLayout };
		}

		private StackLayout MenuItemLayout(string imageSource, string name)
		{
			var menuItemLayout = new StackLayout
			{
				Orientation = StackOrientation.Horizontal,
				Spacing = 15,
				HeightRequest = 40,
				Padding = new Thickness(20, 0),
			};
			if (!string.IsNullOrEmpty(imageSource))
			{
				var image = new Image
				{
					Source = imageSource,
					HeightRequest = 30,
				};
				menuItemLayout.Children.Add(image);
			}
			var nameLabel = new Label
			{
				Style = Styles.Verbosa,
				TextColor = Styles.Colors.NormalText,
				Text = name,
			};
			menuItemLayout.Children.Add(nameLabel);

			return menuItemLayout;
		}
	}
}


