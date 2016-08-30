using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BranchXamarinSDK;
using Exchange.Interfaces;
using Exchange.Pages;
using Exchange.Services;
using Xamarin.Forms;

namespace Exchange
{
	public class App : Application, IBranchSessionInterface
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

		#region IBranchSessionInterface implementation

		public void InitSessionComplete(Dictionary<string, object> data)
		{
			// Do something with the referring link data...
		}

		public void SessionRequestError(BranchError error)
		{
			// Handle the error case here
		}

		#endregion
	}
}

