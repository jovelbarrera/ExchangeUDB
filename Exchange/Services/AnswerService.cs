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
	public class AnswerService
	{
		private static AnswerService _instance;
		public static AnswerService Instance
		{
			get
			{
				if (_instance == null)
					_instance = new AnswerService();
				return _instance;
			}
		}

		private IMobileServiceClient _client;
		private IMobileServiceTable<Answer> _table;

		public AnswerService()
		{
			try
			{
				_client = new MobileServiceClient("http://jovelbarrera.azurewebsites.net");
				_table = _client.GetTable<Answer>();
			}
			catch (Exception ex)
			{
				var a = ex.Message;
			}
		}

		public async Task<List<Answer>> GetQuestionAnswers(Question question)
		{
			var response = await _table.ReadAsync();
			return response?.Where(i => i.QuestionId == question.ObjectId).ToList();
		}
	}
}

