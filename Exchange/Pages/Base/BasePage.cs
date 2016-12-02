using System;
using System.Threading.Tasks;
using Exchange.ContentViews;
using Exchange.Exceptions;
using Exchange.Interfaces;
using Plugin.Connectivity;
using Xamarin.Forms;

namespace Exchange.Pages.Base
{
	public abstract class BasePage : ContentPage
	{
		protected Layout MainLayout;
		protected readonly LoadingContent Loading;
		protected readonly NotFoundContent NotFound;

		protected abstract void InitializeComponents();

		public BasePage()
		{
			Loading = new LoadingContent();
			NotFound = new NotFoundContent();
			InitializeComponents();
			Content = MainLayout;
		}

		public async Task StartAsyncTask(Task task)
		{
			Device.BeginInvokeOnMainThread(() =>
			{
				Content = Loading;
			});
			if (task == null)
				throw new NullReferenceException();
			else if (!CrossConnectivity.Current.IsConnected)
				throw new NoInternetException();
			await task.ConfigureAwait(false);
			Device.BeginInvokeOnMainThread(() =>
			{
				Content = MainLayout;
			});
		}
	}
}

