using System;
using System.Linq;
using Exchange.Dependencies;
using Exchange.Models;
using Exchange.Pages;
using Exchange.Services;
using Realms;
using Xamarin.Forms;
using System.Threading.Tasks;
using Exchange.Interfaces;

namespace Exchange
{
    public class App : Application
    {
        public App()
        {
            MainPage = new LoginPage();
            //InitializeApp().ConfigureAwait(false);
        }

        private async Task InitializeApp()
        {
            IUser currentUser = await UserManager.Instance.GetCurrentUser();
            if (currentUser == null)
                MainPage = new LoginPage();
            else
                MainPage = new MainPage();
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}

