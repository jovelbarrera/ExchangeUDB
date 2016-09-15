using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Exchange.ContentViews;
using Exchange.Models;
using Exchange.Services;
using Exchange.Services.FirebaseServices;
using Kadevjo.Core.Models;
using Plugin.Connectivity;
using Xamarin.Forms;

namespace Exchange.Pages
{
	public partial class EditProfilePage : ContentPage
	{
		private User _user;

		public EditProfilePage(User user)
		{
			_user = user;
			Content = new LoadingContent();
			InitializeComponents();
			ConnectionManager();
		}

		private void ConnectionManager()
		{
			if (!CrossConnectivity.Current.IsConnected)
				Content = new ConnectionFailContent((s, e) => ConnectionManager());
			else
				LoadData().ConfigureAwait(false);
		}

		private async Task LoadData()
		{
			if (_user != null)
			{
				if (!string.IsNullOrEmpty(_user.ProfilePicture))
					_pictureImage.Source = _user.ProfilePicture;
				_nameEntry.Text = _user.DisplayName;
				_emailEntry.Text = _user.Email;
				_universityEntry.Text = (string)_user.GetData("University");
				_careerEntry.Text = (string)_user.GetData("Career");
				_aboutMeEditor.Text = (string)_user.GetData("About");

				Content = _mainLayout;
			}
			else
			{
				Content = new EmptyContent();
			}
		}

		private async void DoneToolbarItem_Clicked(object sender, EventArgs e)
		{
			var user = new PersistentUser
			{
				ObjectId = _user.ObjectId,
				Username = _nameEntry.Text,
				DisplayName = _nameEntry.Text,
				FirstName = _nameEntry.Text,
				LastName = _nameEntry.Text,
				Email = _emailEntry.Text,
				ProfilePicture = _user.ProfilePicture,
				Data = new Dictionary<string, object>
				{
					{"University",_universityEntry.Text},
					{"About",_aboutMeEditor.Text}
				},
			};
			try
			{
				await CustomUserManager.Instance.UpdateCurrentUser(user);
				await DisplayAlert("Perfil actualizado", "Tu perfil ahora está actualizado", "ACEPTAR");
			}
			catch (Exception ex)
			{
				await DisplayAlert("Oops", "Ocurrió un problema al tratar de actualizar tu perfil, por favor intenta de nuevo.", "ACEPTAR");
			}
		}
	}
}

