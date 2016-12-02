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
	public partial class QuestionDetailPage : BaseDetailPage<Question>
	{
		private IUser _currentUser;
		protected ObservableCollection<Answer> Answers;

		#region Constructors

		public QuestionDetailPage() { }

		public QuestionDetailPage(string askId) : base(askId) { }

		public QuestionDetailPage(Question ask) : base(ask) { }

		#endregion

		protected override void Init()
		{
			Answers = new ObservableCollection<Answer>();
            _repliesListview.ItemsSource = Answers;
			base.Init();
		}

		protected override async Task LoadData(string id)
		{
			await base.LoadData(id);
			_currentUser = await CustomUserManager.Instance.GetCurrentUser();

			if (_currentUser.ObjectId == Model.User.ObjectId)
			{
				var editToolbarItem = new ToolbarItem
				{
					Text = "Editar",
					Icon = "ic_edit.png"
				};
				editToolbarItem.Clicked += EditToolbarItem_Clicked;
				ToolbarItems.Add(editToolbarItem);

				var deleteToolbarItem = new ToolbarItem
				{
					Text = "Eliminar",
					Icon = "ic_delete.png"
				};
				deleteToolbarItem.Clicked += DeleteToolbarItem_Clicked;
				ToolbarItems.Add(deleteToolbarItem);
			}

			Content = _mainLayout;
			_titleLabel.Text = Model.Title;
			if (Model.User != null)
				_subtitleLabel.Text = Model.User.Name + "\n" + Model.CreatedAt.HumanDate();
			_descriptionLabel.Text = Model.Description;
			await LoadReplies();
		}

		private async Task LoadReplies()
		{
			_repliesLayout.Children.Insert(0, Loading);
			List<Answer> comments = await AnswerService.Instance.GetQuestionAnswers(Model);
			if (comments != null)
			{
				foreach (var comment in comments)
				{
					if (!string.IsNullOrEmpty(comment.UserId))
					{
						comment.User = await UserService.Instance.GetUser(comment.UserId);
					}
					Answers.Add(comment);
				}
			}

			_responsesLabel.Text = Answers.Count + " Respuestas";

			_repliesLayout.Children.Remove(Loading);
		}

		protected override async Task<Question> RetriveDataById(string id)
		{
			//if (!string.IsNullOrEmpty(id))
			//	return await QuestionService.Instance.Get(id);
			return new Question();
		}

		#region Listeners

		private void RepliesListview_ItemSelected(object sender, SelectedItemChangedEventArgs e)
		{
			if (e.SelectedItem != null)
				return;

			_repliesListview.SelectedItem = null;
		}

		private async void DeleteToolbarItem_Clicked(object sender, EventArgs e)
		{
			bool delete = await DisplayAlert("Confirmar Eliminar",
						 "¿Seguro que quieres eliminar esta pregunta? " +
						 "Es probable que sea útil para otros aún, Eliminala sólo si estás seguro.",
						 "ACEPTAR", "CANCELAR");
			if (delete)
			{
				//if (await QuestionService.Instance.Delete(Model))
				//{
				//	await DisplayAlert("Elimindo", "La pregunta fue eliminada", "ACEPTAR");
				//	await MainPage.Instance.SetAsRootPage(new HomePage());
				//}
			}
		}

		private void EditToolbarItem_Clicked(object sender, EventArgs e)
		{
			//Navigation.PushAsync(new EditQuestion(Model));
		}

		private void ReplyToolbarItem_Clicked(object sender, EventArgs e)
		{
			//Navigation.PushAsync(new EditAnswer(PostAnswer));
		}

		private async void PostAnswer(Answer comment)
		{
            if (comment != null && !string.IsNullOrEmpty(comment.Message))
            {
				_repliesLayout.Children.Insert(0, Loading);

				//await QuestionService.Instance.Reply(Model, comment);
				comment.User.Name = _currentUser.DisplayName;
				comment.User.ProfilePicture = _currentUser.ProfilePicture;
				Answers.Insert(0, comment);
				_responsesLabel.Text = Answers.Count + " Respuestas";

				_repliesLayout.Children.Remove(Loading);

			}
		}

		#endregion
	}
}


