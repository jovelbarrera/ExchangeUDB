using System;
using System.Threading.Tasks;
using Exchange.ContentViews;
using Exchange.Interfaces;
using Plugin.Connectivity;
using Xamarin.Forms;

namespace Exchange.Pages.Base
{
	public abstract class BaseDetailPage<T> : BasePage where T : IModel, new()
	{
		protected T Model { get; set; }

		protected abstract Task<T> RetriveDataById(string id);

		public BaseDetailPage()
		{
			Init();
		}

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

