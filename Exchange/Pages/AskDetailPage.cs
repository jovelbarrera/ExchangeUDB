using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using BranchXamarinSDK;
using Exchange.BranchService;
using Exchange.Configs;
using Exchange.Interfaces;
using Exchange.Models;
using Exchange.Pages.Base;
using Exchange.Services;
using Xamarin.Forms;

namespace Exchange.Pages
{
	public partial class AskDetailPage : BaseDetailPage<Ask>,
										IDeepLinkPage<Ask>,
										IBranchUrlInterface
	{
		private IUser _currentUser;
		protected ObservableCollection<Comment> Comments;

		#region Constructors

		public AskDetailPage() { }

		public AskDetailPage(string askId) : base(askId) { }

		public AskDetailPage(Ask ask) : base(ask) { }

		#endregion

		protected override void Init()
		{
			Comments = new ObservableCollection<Comment>();
			base.Init();
		}

		protected override async Task LoadData(string id)
		{
			await base.LoadData(id);
			_currentUser = await CustomUserManager.Instance.GetCurrentUser();
			Content = _mainLayout;
			_titleLabel.Text = Model.Title;
			if (Model.User != null)
				_subtitleLabel.Text = Model.User.DisplayName + "\n" + Model.CreatedAt.HumanDate();
			_descriptionLabel.Text = Model.Description;
			await LoadReplies();
		}

		private async Task LoadReplies()
		{
			_repliesLayout.Children.Insert(0, Loading);
			List<Comment> comments = await AskService.Instance.GetReplies(Model);
			if (comments != null)
				foreach (var comment in comments)
					Comments.Add(comment);

			_responsesLabel.Text = Comments.Count + " Respuestas";

			_repliesLayout.Children.Remove(Loading);
		}

		protected override async Task<Ask> RetriveDataById(string id)
		{
			if (!string.IsNullOrEmpty(id))
				return await AskService.Instance.Get(id);
			return new Ask();
		}

		#region Listeners

		private void RepliesListview_ItemSelected(object sender, SelectedItemChangedEventArgs e)
		{
			if (e.SelectedItem != null)
				return;

			_repliesListview.SelectedItem = null;
		}

		private void ShareToolbarItem_Clicked(object sender, EventArgs e)
		{
			var data = new Dictionary<string, object>();
			data.Add("objectId", Model.ObjectId);
			var deepLinkMetadata = new DeepLinkMetadata
			{
				Title = "ExchangeUDB",
				Description = "Descarga la app",
				ImageUrl = "https://raw.githubusercontent.com/rejbarrera/ExchangeUDB/master/Droid/Resources/drawable-mdpi/ic_exchange_logo.png",

			};
			BranchService<Ask, AskDetailPage>.Instance.GenerateLink(data, deepLinkMetadata);
		}

		private async void DeleteToolbarItem_Clicked(object sender, EventArgs e)
		{
			bool delete = await DisplayAlert("Confirmar Eliminar",
						 "¿Seguro que quieres eliminar esta pregunta? " +
						 "Es probable que sea útil para otros aún, Eliminala sólo si estás seguro.",
						 "ACEPTAR", "CANCELAR");
			if (delete)
			{
				if (await AskService.Instance.Delete(Model))
				{
					await DisplayAlert("Elimindo", "La pregunta fue eliminada", "ACEPTAR");
					await MainPage.Instance.SetAsRootPage(new HomePage());
				}
			}
		}

		private void EditToolbarItem_Clicked(object sender, EventArgs e)
		{
			Navigation.PushAsync(new EditAsk(Model));
		}

		private void ReplyToolbarItem_Clicked(object sender, EventArgs e)
		{
			Navigation.PushAsync(new EditComment(PostComment));
		}

		private async void PostComment(Comment comment)
		{
			if (comment != null)
			{
				_repliesLayout.Children.Insert(0, Loading);

				await AskService.Instance.Reply(Model, comment);
				comment.User.DisplayName = _currentUser.DisplayName;
				comment.User.ProfilePicture = _currentUser.ProfilePicture;
				Comments.Insert(0, comment);
				_responsesLabel.Text = Comments.Count + " Respuestas";

				_repliesLayout.Children.Remove(Loading);

			}
		}

		#endregion

		#region IBranchUrlInterface implementation

		public void ReceivedUrl(string uri)
		{
			throw new NotImplementedException();
		}

		public void UrlRequestError(BranchError error)
		{
			throw new NotImplementedException();
		}

		#endregion

		#region IDeepLinkPage

		public Task OnDeeplink(Ask model)
		{
			throw new NotImplementedException();
		}

		#endregion
	}
}


