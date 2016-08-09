using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Exchange.Configs;
using Exchange.Helpers;
using Exchange.Models;
using Exchange.Services.FirebaseServices;

namespace Exchange.Services.FirebaseServices
{
	public class UserService : FirebaseDatabaseService<User>
	{
		private static UserService _instance;
		public static UserService Instance
		{
			get
			{
				if (_instance == null)
					_instance = new UserService();
				return _instance;
			}
		}

		protected override IFirebaseAccess FirebaseAccess { get { return Configs.FirebaseAccess.Instance; } }

		protected override string BaseResourcePath { get { return "User"; } }

		protected override string Token { get { return Settings.FirebaseUserToken; } }

		public async Task Register(User user)
		{
			DateTime currentTime = await TimeService.Instance.Now();
			try
			{
				var registeredUser = await ReadSingle(user.ObjectId);
				user.UpdatedAt = currentTime;
				if (registeredUser == null)
				{
					user.CreatedAt = currentTime;
					await base.Create(user);
				}
				else
				{
					user.CreatedAt = registeredUser.CreatedAt;
					await base.Update(user);
				}
			}
			catch (HttpRequestException ex)
			{
				if (ex.Message == "401 (Unauthorized)")
					await App.Current.MainPage.DisplayAlert("Error", "No tienes permisos para completar esta acción", "ACEPTAR");
				if (ex.Message == "400 (Bad Request)")
					await App.Current.MainPage.DisplayAlert("Error", "Un error ocurrió al intentar realizar esta acción", "ACEPTAR");
				else
					await App.Current.MainPage.DisplayAlert("Error", ex.Message, "ACEPTAR");
			}
			catch (Exception ex)
			{
				await App.Current.MainPage.DisplayAlert("Error", ex.Message, "ACEPTAR");
			}
		}

		public async Task<User> Get(string objectId)
		{
			return await ReadSingle(objectId);
		}
	}
}

