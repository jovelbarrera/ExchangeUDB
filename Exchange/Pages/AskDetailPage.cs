using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Exchange.Configs;
using Exchange.ContentViews;
using Exchange.Models;
using Exchange.Services;
using Exchange.Services.FirebaseServices;
using Plugin.Connectivity;
using Xamarin.Forms;
using Exchange.Interfaces;

namespace Exchange.Pages
{
    public partial class AskDetailPage : BaseDetailPage<Ask>
    {
        private IUser _currentUser;
        protected ObservableCollection<Comment> Comments;

        #region Constructors

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
            _currentUser = await UserManager.Instance.GetCurrentUser();
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

        //if (_ask != null)
        //		{
        //			InitializeComponents();
        //			ConnectionManager();
        //			return;
        //		}
        //
        //Content = new NotFoundContent();

        #region Listeners

        private void RepliesListview_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem != null)
                return;

            _repliesListview.SelectedItem = null;
        }

        private void ShareToolbarItem_Clicked(object sender, EventArgs e)
        {
            // TODO
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
    }
}


