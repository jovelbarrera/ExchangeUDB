using System;
using System.Threading.Tasks;
using Exchange.ContentViews;
using Exchange.Interfaces;
using Plugin.Connectivity;
using Xamarin.Forms;

namespace Exchange.Pages
{
	public abstract class BaseDetailPage<T> : ContentPage where T : IModel, new()
	{
		protected readonly LoadingContent Loading = new LoadingContent();
		protected readonly NotFoundContent NotFound = new NotFoundContent();

		protected T Model { get; set; }

		protected abstract void InitializeComponents();
		protected abstract Task<T> RetriveDataById(string id);

		public BaseDetailPage(string id)
		{
			Init();
			LoadData(id).ConfigureAwait(false);
		}

		public BaseDetailPage(T model)
		{
			Init();
			Model = model;
			LoadData(null).ConfigureAwait(false);
		}

		protected virtual void Init()
		{
			Model = new T();
			InitializeComponents();
		}

		protected virtual async Task LoadData(string id)
		{
			if (!string.IsNullOrEmpty(id))
			{
				Content = Loading;
				Model = await RetriveDataById(id);
			}
		}
	}
}

