using System;
using System.Linq;
using Exchange.Dependencies;
using Exchange.Models;
using Exchange.Pages;
using Exchange.Services;
using Realms;
using Xamarin.Forms;
using System.Threading.Tasks;

namespace Exchange
{
    public class App : Application
    {
        public App()
        {
            MainPage = new LoginPage();
            //if (UserManager.Instance.CurrentUser == null)
            //	MainPage = new LoginPage();			
            //else
            //	MainPage = new MainPage();
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

