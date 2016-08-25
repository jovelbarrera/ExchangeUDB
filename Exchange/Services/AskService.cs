using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Exchange.Helpers;
using Exchange.Models;
using Exchange.Services.FirebaseServices;
using Kadevjo.Core.Models;

namespace Exchange.Services
{
	public class AskService : FirebaseDatabaseService<AskService, Ask>
	{
		protected override string Resource { get { return "Ask"; } }

		protected override string Token { get { return Settings.FirebaseUserToken; } }

		protected override string BaseUrl { get { return Configs.FirebaseAccess.Instance.FirebaseBasePath; } }

		protected override Dictionary<string, string> Headers { get { return new Dictionary<string, string>(); } }

		public async Task<string> Create(Ask ask)
		{
			GenericResponse<Dictionary<string, string>> response = await base.Create<Dictionary<string, string>>(ask);
			if (response != null && response.Model != null && response.Model.ContainsKey("name"))
				return response.Model["name"];
			return null;
		}

		public async Task<List<Ask>> GetNext(int limit, string readFromId)
		{
			var query = new FirebaseQuery();
			query.OrderBy("$key");
			query.LimitToFirst(limit);
			query.StartAt(readFromId);
			return await base.ReadAll(query);
		}

		public async Task<List<Ask>> GetPrev(int limit, string readFromId)
		{
			var query = new FirebaseQuery();
			query.OrderBy("$key");
			query.LimitToFirst(limit);
			query.EndAt(readFromId);
			return await base.ReadAll(query);
		}

		public async Task<List<Ask>> GetLatest(int limit = 10)
		{
			var query = new FirebaseQuery();
			query.OrderBy("$key");
			query.LimitToLast(limit);
			return await base.ReadAll(query);
		}

		public async Task<Ask> Get(string objectId)
		{
			return await base.Read(objectId);
		}

		public async Task<bool> Update(Ask model)
		{
			GenericResponse<Dictionary<string, string>> response = await Update<Dictionary<string, string>>(model);
			return response.Status.IsSuccessStatusCode;
		}

		public async Task<bool> Delete(Ask model)
		{
			GenericResponse<object> response = await Delete<object>(model.ObjectId);
			return response.Status.IsSuccessStatusCode;
			//return await RequestHandler<bool>.Factory(base.Delete(model)).Execute();
		}

		public async Task<string> Reply(Ask model, Comment comment)
		{
			string resource = string.Format("{0}/{1}/Comments", Resource, model.ObjectId);
			if (model != null && !string.IsNullOrEmpty(model.ObjectId))
			{
				GenericResponse<Dictionary<string, string>> response = await Create<Dictionary<string, string>, Comment>(comment, resource);
				if (response != null && response.Model != null && response.Model.ContainsKey("name"))
					return response.Model["name"];
			}
			return null;
		}

		public async Task<List<Comment>> GetReplies(Ask model)
		{
			var comments = new List<Comment>();
			if (model != null && !string.IsNullOrEmpty(model.ObjectId))
			{
				string resource = string.Format("{0}/{1}/Comments", Resource, model.ObjectId);
				var query = new FirebaseQuery();
				query.OrderBy("$key");
				query.LimitToLast(10);
				comments = await base.ReadAll<Comment>(resource, query);
				if (comments != null && comments.Count > 0)
				{
					foreach (var comment in comments)
					{
						if (comment.User != null && !string.IsNullOrEmpty(comment.User.ObjectId))
							comment.User = await UserService.Instance.Get(comment.User.ObjectId);
					}
				}
			}

			return comments;
		}
	}
}

