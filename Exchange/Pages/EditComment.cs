using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Exchange.Configs;
using Exchange.ContentViews;
using Exchange.Controls;
using Exchange.Interfaces;
using Exchange.Models;
using Exchange.Services;
using Xamarin.Forms;

namespace Exchange.Pages
{
	public partial class EditComment : ContentPage
	{
		private const int MaxCharsOnDecription = 500;

		private Comment _comment;
		private bool _isNewComment;
		private IUser _currentUser;
		private bool _sending;
		private bool _hasEditedMessage;

		public Action<Comment> OnComplete { get; private set; }

		public EditComment(Action<Comment> onComplete, Comment comment = null)
		{
			Init(onComplete, comment);
			InitializeComponents();
			LoadData();
		}

		private void Init(Action<Comment> onComplete, Comment comment)
		{
			_isNewComment = comment == null;
			_comment = comment ?? new Comment();
			OnComplete = onComplete;
			_currentUser = UserManager.Instance.CurrentUser;
		}

		private void LoadData()
		{
			if (!string.IsNullOrEmpty(_currentUser.ProfilePicture))
				_pictureImage.Source = _currentUser.ProfilePicture;

			if (!_isNewComment)
			{
				Title = "Editar Comentario";

				_hasEditedMessage = true;
				_messageEditor.Text = _comment.Message;
				_messageEditor.TextColor = Styles.Colors.NormalText;
			}
		}

		#region Post Logic
		private void DoneToolbarItem_Clicked(object sender, EventArgs e)
		{
			if (!_sending)
			{
				DisableControls();
				_sending = true;
				if (IsValidData())
				{
					_comment.Message = _messageEditor.Text.Trim();
					_comment.User = new User { ObjectId = _currentUser.ObjectId };
					Navigation.PopAsync();
				}
				EnableControls();
				_sending = false;
			}
		}

		private bool IsValidData()
		{
			if (string.IsNullOrEmpty(_messageEditor.Text) || !_hasEditedMessage)
			{
				DisplayAlert("Comentario vacío", "Debes agregar una descripción a tu pregunta", "ACEPTAR");
				return false;
			}
			return true;
		}

		private void DisableControls()
		{
			_messageEditor.IsEnabled = false;
		}

		private void EnableControls()
		{
			_messageEditor.IsEnabled = true;
		}

		#endregion

		#region Message Logic

		private void MessageEditor_TextChanged(object sender, TextChangedEventArgs e)
		{
			if (e.NewTextValue.Length > MaxCharsOnDecription)
				_messageEditor.Text = e.OldTextValue;

			if (_hasEditedMessage)
				_chractersLeftLabel.Text = "@" + (MaxCharsOnDecription - e.NewTextValue.Length).ToString();
			else
				_chractersLeftLabel.Text = "@" + MaxCharsOnDecription;
		}

		private void MessageEditor_Unfocused(object sender, FocusEventArgs e)
		{
			if (string.IsNullOrEmpty(_messageEditor.Text))
			{
				_hasEditedMessage = false;
				_messageEditor.Text = "Escribe tu comentario";
				_messageEditor.TextColor = Styles.Colors.Subtitle;
			}
		}

		private void MessageEditor_Focused(object sender, FocusEventArgs e)
		{
			if (!_hasEditedMessage)
			{
				_hasEditedMessage = true;
				_messageEditor.Text = string.Empty;
				_messageEditor.TextColor = Styles.Colors.NormalText;
			}
		}

		#endregion

		#region Dissmiss

		protected override bool OnBackButtonPressed()
		{
			DiscartComment().ConfigureAwait(false);
			return true;
		}

		private async Task DiscartComment()
		{
			if (_hasEditedMessage)
			{
				bool dismiss = await DisplayAlert("Descartar comentario", "¿Seguro que quieres descartar este comentario?", "DESCARTAR", "CANCELAR");
				if (dismiss)
					await Navigation.PopAsync();
			}
			else
			{
				await Navigation.PopAsync();
			}
		}

		protected override void OnDisappearing()
		{
			base.OnDisappearing();
			if (OnComplete != null)
				OnComplete(_comment);
		}

		#endregion
	}
}


