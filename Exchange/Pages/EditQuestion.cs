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
	public partial class EditQuestion : ContentPage
	{
		private const int MaxCharsOnDecription = 500;
		private const int MaxCharsOnTitle = 100;

		private Question _ask;
		private bool _isNewQuestion;
		private IUser _currentUser;
		private bool _sending;
		private bool _hasEditedDescription;

		public EditQuestion(Question ask = null)
		{
			Init(ask).ConfigureAwait(false);
			InitializeComponents();
		}

		private async Task Init(Question ask)
		{
			_isNewQuestion = ask == null;
			_ask = ask ?? new Question();
			_currentUser = await CustomUserManager.Instance.GetCurrentUser();
			LoadData();
		}

		private void LoadData()
		{
			if (!string.IsNullOrEmpty(_currentUser.ProfilePicture))
				_pictureImage.Source = _currentUser.ProfilePicture;

			if (!_isNewQuestion)
			{
				Title = "Editar Pregunta";
				_titleEntry.Text = _ask.Title;

				_hasEditedDescription = true;
				_descriptionEditor.Text = _ask.Description;
				_descriptionEditor.TextColor = Styles.Colors.NormalText;

			}
		}

		private void TitleEntry_TextChanged(object sender, TextChangedEventArgs e)
		{
			if (e.NewTextValue.Length > MaxCharsOnTitle)
				_titleEntry.Text = e.OldTextValue;
		}

		#region Post Logic
		private async void DoneToolbarItem_Clicked(object sender, EventArgs e)
		{
			if (!_sending)
			{
				DisableControls();
				_sending = true;
				if (IsValidData())
				{
					Content = new LoadingContent();
					_ask.Title = _titleEntry.Text.Trim();
					_ask.Description = _descriptionEditor.Text.Trim();

					_ask.User = new AppUser { ObjectId = _currentUser.ObjectId };
					string askId = _ask.ObjectId;
					if (_isNewQuestion)
						askId = await QuestionService.Instance.Create(_ask);
					else
						await QuestionService.Instance.Update(_ask);

					_ask.ObjectId = askId;
					_ask.User.Name = _currentUser.DisplayName;
					_ask.CreatedAt = DateTime.Now;

					if (string.IsNullOrEmpty(askId))
						await DisplayAlert("Error", "No se pudo publicar tu pregunta", "ACEPTAR");
					else
						await MainPage.Instance.SetAsRootPage(new QuestionDetailPage(_ask));

					Content = _mainLayout;
				}
				EnableControls();
				_sending = false;
			}
		}

		private bool IsValidData()
		{
			if (string.IsNullOrEmpty(_titleEntry.Text))
			{
				DisplayAlert("Pregunta sin título", "Debes asignar un título a tu pregunta", "ACEPTAR");
				return false;
			}
			else if (string.IsNullOrEmpty(_descriptionEditor.Text) || !_hasEditedDescription)
			{
				DisplayAlert("Pregunta sin descripción", "Debes agregar una descripción a tu pregunta", "ACEPTAR");
				return false;
			}
			return true;
		}

		private void DisableControls()
		{
			_titleEntry.IsEnabled = false;
			_descriptionEditor.IsEnabled = false;
		}

		private void EnableControls()
		{
			_titleEntry.IsEnabled = true;
			_descriptionEditor.IsEnabled = true;
		}

		#endregion

		#region Description Logic

		private void DescriptionEditor_TextChanged(object sender, TextChangedEventArgs e)
		{
			if (e.NewTextValue.Length > MaxCharsOnDecription)
				_descriptionEditor.Text = e.OldTextValue;

			if (_hasEditedDescription)
				_chractersLeftLabel.Text = "@" + (MaxCharsOnDecription - e.NewTextValue.Length).ToString();
			else
				_chractersLeftLabel.Text = "@" + MaxCharsOnDecription;
		}

		private void DescriptionEditor_Unfocused(object sender, FocusEventArgs e)
		{
			if (string.IsNullOrEmpty(_descriptionEditor.Text))
			{
				_hasEditedDescription = false;
				_descriptionEditor.Text = "Escribe tu pregunta";
				_descriptionEditor.TextColor = Styles.Colors.Subtitle;
			}
		}

		private void DescriptionEditor_Focused(object sender, FocusEventArgs e)
		{
			if (!_hasEditedDescription)
			{
				_hasEditedDescription = true;
				_descriptionEditor.Text = string.Empty;
				_descriptionEditor.TextColor = Styles.Colors.NormalText;
			}
		}

		#endregion
	}
}


