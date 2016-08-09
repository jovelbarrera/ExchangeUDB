using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Exchange.Configs;
using Exchange.ContentViews;
using Exchange.Models;
using Plugin.Connectivity;
using Xamarin.Forms;

namespace Exchange.Pages
{
	public partial class ExchangeDetailPage : ContentPage
	{
		private Models.Exchange _exchange;

		#region Constructors
		public ExchangeDetailPage(string exchangeId)
		{
			Content = new LoadingContent();
			RetriveDataById(exchangeId).ConfigureAwait(false);
		}

		public ExchangeDetailPage(Models.Exchange exchange)
		{
			Content = new LoadingContent();
			_exchange = exchange;
			InitializeComponents();
			ConnectionManager();
		}
		#endregion
		private void ConnectionManager()
		{
			if (!CrossConnectivity.Current.IsConnected)
				Content = new ConnectionFailContent((s, e) => ConnectionManager());
			else
				LoadData().ConfigureAwait(false);
		}

		private async Task LoadData()
		{
			_thumbnailImage.Source = _exchange.Thumbnail;
			_titleLabel.Text = _exchange.Title;
			_subtitleLabel.Text = _exchange.User.DisplayName + "\n" + _exchange.CreatedAt;
			_descriptionLabel.Text = _exchange.Description;

			List<Comment> itemsSource = await Dummy.CommentList();
			_responsesLabel.Text = itemsSource.Count + " Comentarios";
			_commentsList.ItemsSource = itemsSource;

			Content = _mainLayout;
		}

		private async Task RetriveDataById(string id)
		{
			if (!string.IsNullOrEmpty(id))
			{
				_exchange = null;// TODO call service to retrive Ask by Id
				if (_exchange != null)
				{
					InitializeComponents();
					ConnectionManager();
					return;
				}
			}
			Content = new NotFoundContent();
		}
	}
}


