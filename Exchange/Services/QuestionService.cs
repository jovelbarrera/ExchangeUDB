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
	public class QuestionService
	{
		private static QuestionService _instance;
		public static QuestionService Instance
		{
			get
			{
				if (_instance == null)
					_instance = new QuestionService();
				return _instance;
			}
		}

		private IMobileServiceClient _client;
		private IMobileServiceTable<Question> _table;

		public QuestionService()
		{
			try
			{
				_client = new MobileServiceClient("http://jovelbarrera.azurewebsites.net");
				_table = _client.GetTable<Question>();
			}
			catch (Exception ex)
			{
				var a = ex.Message;
			}
		}

		public async Task<List<Question>> GetLatest()
		{
			var response = await _table.ToEnumerableAsync();
			return response?.ToList();
		}

		public async Task<string> Create(Question question)
		{
			await _table.InsertAsync(question);
			return "";
		}

		public async Task Update(Question question)
		{
			await _table.UpdateAsync(question);
		}
	}
}

