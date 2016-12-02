using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Exchange.Configs;
using Exchange.Helpers;
using Exchange.Models;
using Exchange.Services.FirebaseServices;
using Kadevjo.Core.Models;
using Microsoft.WindowsAzure.MobileServices;

namespace Exchange.Services
{
	public class UserService
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

		private IMobileServiceClient _client;
		private IMobileServiceTable<AppUser> _table;

		public UserService()
		{
			try
			{
				_client = new MobileServiceClient("http://jovelbarrera.azurewebsites.net");
				_table = _client.GetTable<AppUser>();
			}
			catch (Exception ex)
			{
				var a = ex.Message;
			}
		}

		public async Task<AppUser> GetUser(string id)
		{
			string query = "SELECT * WHERE id ='" + id + "'";
			var response = await _table.ReadAsync();
			return response?.Where(i => i.ObjectId == id).FirstOrDefault();
		}
	}
}

