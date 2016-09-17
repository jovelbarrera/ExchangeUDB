using Exchange.Controls;
using Exchange.Models;
using Exchange.Pages.Base;
using Exchange.Services;
using System;
using Xamarin.Forms;

namespace Exchange.Pages
{
    public class SettingsPage : BasePage
    {
        public SettingsPage()
        {
            Title = "Configuración";
            //LoadSettings().ConfigureAwait(false);
        }

        /*private async Task LoadSettings()
		{
			IUser currentUser = await CustomUserManager.Instance.GetCurrentUser();
			Children.Add(new ProfilePage(new User(currentUser)));
			Children.Add(new ActivityPage());
			Children.Add(new EditProfilePage(new User(currentUser)));
		}*/
        protected override void InitializeComponents()
        {
            var table = new CustomTableView
            {
                Intent = TableIntent.Settings,
                RowHeight = 60,
            };

            TextCell profileCell = CellLayout("Mi Perfil", "Visualiza tu perfil público", PushProfile);
            TextCell editProfileCell = CellLayout("Editar Perfil", "Actualiza la información de tu perfil", PushEditProfile);
            TextCell logoutCell = CellLayout("Cerrar sesión", "Desvincula tu cuenta de este dispositivo", Logout);

            TextCell activityCell = CellLayout("Historial de actividad", "Revisa tu actividad en Exchange", PushHistorial);

            table.Root = new TableRoot {
                new TableSection("Cuenta") {
                    profileCell,
                    editProfileCell,
                    logoutCell
                },
                new TableSection("Actividad") {
                    activityCell,
                }
            };
            MainLayout = new ContentView { Content = table };
        }

        private async void PushProfile(object sender, EventArgs e)
        {
            User currentUser = Configs.Dummy.User(); //new User(await CustomUserManager.Instance.GetCurrentUser());
            await Navigation.PushAsync(new ProfilePage(currentUser), true);
        }

        private async void PushEditProfile(object sender, EventArgs e)
        {
            User currentUser = Configs.Dummy.User(); //new User(await CustomUserManager.Instance.GetCurrentUser());
            await Navigation.PushAsync(new EditProfilePage(currentUser), true);
        }

        private async void Logout(object sender, EventArgs e)
        {
            await CustomUserManager.Instance.DeleteCurrentUser();
            App.Current.MainPage = new LoginPage();
        }

        private async void PushHistorial(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new ActivityPage(), true);
        }

        private TextCell CellLayout(string text, string detail, EventHandler action)
        {
            var textCell = new TextCell
            {
                Text = text,
                TextColor = Color.FromHex("#6F6F6F"),
                Detail = detail,
                DetailColor = Color.FromHex("#B7B7B7"),
            };
            textCell.Tapped += action;
            return textCell;
        }
    }
}

