using System.Collections.Generic;
using System.Threading.Tasks;
using Exchange.Configs;
using Exchange.Helpers;
using Exchange.Models;
using Exchange.Services.FirebaseServices;
using Kadevjo.Core.Models;
using Exchange.Interfaces;

namespace Exchange.Services
{
	public class VideoService : FirebaseDatabaseService<VideoService, Video>
	{
		protected override string Resource { get { return "Video"; } }

		protected override FirebaseToken Token { get { return CurrentFirebaseToken(); } }

		protected override string BaseUrl { get { return FirebaseAccess.Instance.FirebaseBasePath; } }

		protected override Dictionary<string, string> Headers { get { return new Dictionary<string, string>(); } }

		/*public async Task<string> Create(Video ask)
		{
			GenericResponse<Dictionary<string, string>> response = await base.Create<Dictionary<string, string>>(ask);
			if (response != null && response.Model != null && response.Model.ContainsKey("name"))
				return response.Model["name"];
			return null;
		}*/

		public async Task<List<Video>> GetNext(int limit, string readFromId)
		{
			var query = new FirebaseQuery();
			query.OrderBy("$key");
			query.LimitToFirst(limit);
			query.StartAt(readFromId);
			GenericResponse<List<Video>> response = await base.ReadAll(query);
			return response.Model;
		}

		public async Task<List<Video>> GetPrev(int limit, string readFromId)
		{
			var query = new FirebaseQuery();
			query.OrderBy("$key");
			query.LimitToFirst(limit);
			query.EndAt(readFromId);
			GenericResponse<List<Video>> response = await base.ReadAll(query);
			return response.Model;
		}

		public async Task<List<Video>> GetLatest(int limit = 10)
		{
			var query = new FirebaseQuery();
			query.OrderBy("$key");
			query.LimitToLast(limit);
			GenericResponse<List<Video>> response = await base.ReadAll(query);
			return response.Model;
		}

		public async Task<Video> Get(string objectId)
		{
			GenericResponse<Video> result = await base.Read(objectId);
			return result.Model;
		}

        /*public async Task<bool> Update(Video model)
		{
			GenericResponse<Dictionary<string, string>> response = await Update<Dictionary<string, string>>(model);
			return response.Status.IsSuccessStatusCode;
		}

		public async Task<bool> Delete(Video model)
		{
			GenericResponse<object> response = await Delete<object>(model.ObjectId);
			return response.Status.IsSuccessStatusCode;
			//return await RequestHandler<bool>.Factory(base.Delete(model)).Execute();
		}*/

        public async Task<string> Like(Video model, User user)
        {
            string resource = string.Format("{0}/{1}/Likes", Resource, model.ObjectId);
            if (model != null && !string.IsNullOrEmpty(model.ObjectId))
            {
                GenericResponse<Dictionary<string, string>> response = await Create<Dictionary<string, string>, User>(user, resource);
                if (response != null && response.Model != null && response.Model.ContainsKey("name"))
                    return response.Model["name"];
            }
            return null;
        }

        public async Task<bool> DeleteLike(Video model, string likeId)
        {
            string resource = string.Format("{0}/{1}/Likes", Resource, model.ObjectId);
            if (model != null && !string.IsNullOrEmpty(model.ObjectId))
            {
                GenericResponse<object> response = await Delete<object, Video>(likeId, resource);
                return response.Status.IsSuccessStatusCode;
            }
            return false;
        }

        /*public async Task<List<Comment>> GetLikes(Video model)
        {
            var comments = new List<Comment>();
            if (model != null && !string.IsNullOrEmpty(model.ObjectId))
            {
                string resource = string.Format("{0}/{1}/Likes", Resource, model.ObjectId);
                GenericResponse<List<Comment>> result = await base.ReadAll<Comment>(resource);
                comments = result.Model;
            }

            return comments;
        }*/

        public async Task<string> Reply(Video model, Comment comment)
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

		public async Task<List<Comment>> GetReplies(Video model)
		{
			var comments = new List<Comment>();
			if (model != null && !string.IsNullOrEmpty(model.ObjectId))
			{
				string resource = string.Format("{0}/{1}/Comments", Resource, model.ObjectId);
				var query = new FirebaseQuery();
				query.OrderBy("$key");
				query.LimitToLast(10);
				GenericResponse<List<Comment>> result = await base.ReadAll<Comment>(resource, query);
				comments = result.Model;
				if (comments != null && comments.Count > 0)
				{
					foreach (var comment in comments)
					{
						if (comment.User != null && !string.IsNullOrEmpty(comment.User.ObjectId))
						{
							GenericResponse<User> userResult = await FirebaseUserService.Instance.Read(comment.User.ObjectId);
							comment.User = userResult.Model;
						}
					}
				}
			}

			return comments;
		}
	}
}

