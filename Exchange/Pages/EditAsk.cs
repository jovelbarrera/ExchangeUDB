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
	public partial class EditAsk : ContentPage
	{
		private const int MaxCharsOnDecription = 500;
		private const int MaxCharsOnTitle = 100;

		private Ask _ask;
		private bool _isNewAsk;
		private IUser _currentUser;
		private List<string> _tags;
		private bool _sending;
		private bool _hasEditedDescription;

		public EditAsk(Ask ask = null)
		{
			Init(ask);
			InitializeComponents();
			LoadData();
		}

		private void Init(Ask ask)
		{
			_isNewAsk = ask == null;
			_ask = ask ?? new Ask();
			_tags = new List<string>();
			_currentUser = UserManager.Instance.CurrentUser;
		}

		private void LoadData()
		{
			if (!string.IsNullOrEmpty(_currentUser.ProfilePicture))
				_pictureImage.Source = _currentUser.ProfilePicture;

			if (!_isNewAsk)
			{
				Title = "Editar Pregunta";
				_titleEntry.Text = _ask.Title;

				_hasEditedDescription = true;
				_descriptionEditor.Text = _ask.Description;
				_descriptionEditor.TextColor = Styles.Colors.NormalText;

				foreach (var tag in _ask.Tags)
					AddTag(tag);
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
					_ask.Tags = _tags.ToArray();
					_ask.User = new User { ObjectId = _currentUser.ObjectId };
					string askId = _ask.ObjectId;
					if (_isNewAsk)
						askId = await AskService.Instance.Create(_ask);
					else
						await AskService.Instance.Update(_ask);

					_ask.ObjectId = askId;
					_ask.User.DisplayName = _currentUser.DisplayName;
					_ask.CreatedAt = DateTime.Now;

					if (string.IsNullOrEmpty(askId))
						await DisplayAlert("Error", "No se pudo publicar tu pregunta", "ACEPTAR");
					else
						await MainPage.Instance.SetAsRootPage(new AskDetailPage(_ask));

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
			else if (_tags.Count < 1)
			{
				DisplayAlert("Pregunta sin tag", "Debes agregar al menos una tag relacionada a tu pregunta", "ACEPTAR");
				return false;
			}
			return true;
		}

		private void DisableControls()
		{
			_titleEntry.IsEnabled = false;
			_descriptionEditor.IsEnabled = false;
			_tagsEntry.IsEnabled = false;
		}

		private void EnableControls()
		{
			_titleEntry.IsEnabled = true;
			_descriptionEditor.IsEnabled = true;
			_tagsEntry.IsEnabled = true;
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

		#region Tags Logic

		private void TagsEntry_TextChanged(object sender, TextChangedEventArgs e)
		{
			bool isValidTag = Utils.IsValidTag(e.NewTextValue);

			if (Utils.IsValidTag(e.OldTextValue) && e.NewTextValue.EndsWith(","))
				AddTag(e.OldTextValue);
			else if (Utils.IsValidTag(e.NewTextValue))
				_tagsEntry.TextColor = Styles.Colors.NormalText;
			else
				_tagsEntry.TextColor = Color.Red;
		}

		private void TagsEntry_Completed(object sender, EventArgs e)
		{
			string tagString = ((CustomEntry)sender).Text;

			if (Utils.IsValidTag(tagString))
				AddTag(tagString);
			else
				_tagsEntry.TextColor = Color.Red;
		}

		private void AddTag(string newtag)
		{
			if (!_tags.Any(i => i.ToLower() == newtag.ToLower()))
			{
				_tagsEntry.TextColor = Styles.Colors.NormalText;
				_tagsLayout.Children.Add(TagLayout(newtag));
				_tagsEntry.Text = string.Empty;
				_tags.Add(newtag);
			}
			else
			{
				_tagsEntry.TextColor = Color.Red;
			}
		}

		private void RemoveTag(string tag)
		{
			if (!string.IsNullOrEmpty(tag))
				_tags.Remove(tag);
		}

		#endregion
	}
}


