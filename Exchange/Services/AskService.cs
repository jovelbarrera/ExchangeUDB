using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Exchange.Helpers;
using Exchange.Models;
using Exchange.Services.FirebaseServices;

namespace Exchange.Services
{
	public class AskService : FirebaseDatabaseService<Ask>
	{
		private static AskService _instance;
		public static AskService Instance
		{
			get
			{
				if (_instance == null)
					_instance = new AskService();
				return _instance;
			}
		}

		protected override IFirebaseAccess FirebaseAccess { get { return Configs.FirebaseAccess.Instance; } }

		protected override string BaseResourcePath { get { return "Ask"; } }

		protected override string Token { get { return Settings.FirebaseUserToken; } }

		public async Task<string> Create(Ask ask)
		{

			//DateTime currentTime = await TimeService.Instance.Now();
			//ask.CreatedAt = currentTime;
			//ask.UpdatedAt = currentTime;

			//string title = ask.Title;
			//for (int i = 1; i < 50; i++)
			//{
			//	ask.Title = title + " " + i;
			//	ask.CreatedAt = DateTime.Now.AddHours(i);
			//	ask.UpdatedAt = DateTime.Now.AddHours(i);
			//	await base.Create(ask);
			//}

			return await RequestHandler<string>.Factory(base.Create(ask)).Execute();
		}

		public async Task<List<Ask>> GetNext(int limit, string readFromId)
		{
			return await RequestHandler<List<Ask>>.Factory(ReadNext(limit, readFromId)).Execute();
		}

		public async Task<List<Ask>> GetPrev(int limit, string readFromId)
		{
			return await RequestHandler<List<Ask>>.Factory(ReadPrev(limit, readFromId)).Execute();
		}

		public async Task<List<Ask>> GetLatest(int limit = 10)
		{
			return await RequestHandler<List<Ask>>.Factory(ReadLimitToLast(limit)).Execute();
		}

		public async Task<Ask> Get(string objectId)
		{
			Ask ask = await RequestHandler<Ask>.Factory(ReadSingle(objectId)).Execute();
			if (ask != null && !string.IsNullOrEmpty(ask.ObjectId))
				ask.User = await UserService.Instance.Get(ask.User.ObjectId);
			return ask;
		}

		public async Task<bool> Update(Ask model)
		{
			return await RequestHandler<bool>.Factory(base.Update(model)).Execute();
		}

		public async Task<bool> Delete(Ask model)
		{
			return await RequestHandler<bool>.Factory(base.Delete(model)).Execute();
		}

		public async Task<string> Reply(Ask model, Comment comment)
		{
			if (model != null && !string.IsNullOrEmpty(model.ObjectId))
				return await RequestHandler<string>.Factory(base.Create<Comment>(comment, model.ObjectId + "/Comments")).Execute();
			return null;
		}

		public async Task<List<Comment>> GetComments(Ask model)
		{
			var comments = new List<Comment>();
			if (model != null && !string.IsNullOrEmpty(model.ObjectId))
			{
				comments = await RequestHandler<List<Comment>>.Factory(ReadLimitToFirst<Comment>(10, model.ObjectId + "/Comments")).Execute();
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

