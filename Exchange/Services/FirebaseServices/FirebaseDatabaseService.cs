using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Exchange.Configs;
using Exchange.Interfaces;
using Firebase.Xamarin.Database;
using Firebase.Xamarin.Database.Query;

namespace Exchange.Services.FirebaseServices
{
	public abstract class FirebaseDatabaseService<T> where T : IModel, new()
	{
		private FirebaseClient _firebaseClient;
		protected abstract string BaseResourcePath { get; }
		protected abstract string Token { get; }
		protected abstract IFirebaseAccess FirebaseAccess { get;}

		public FirebaseDatabaseService()
		{
			_firebaseClient = new FirebaseClient(FirebaseAccess.FirebaseBasePath);
		}


		#region private methods

		private ChildQuery BaseChildQuery(string childResource)
		{
			return _firebaseClient.Child(FullResourcePath(childResource));
		}

		private OrderQuery BaseFilterQuery(string childResource)
		{
			return BaseChildQuery(childResource).OrderByKey();
		}

		private async Task<List<I>> Once<I>(FilterQuery baseFilterQuery)
			where I : IModel
		{
			IReadOnlyCollection<FirebaseObject<I>> results;
			if (!string.IsNullOrEmpty(Token))
				results = await baseFilterQuery.WithAuth(Token).OnceAsync<I>();
			else
				results = await baseFilterQuery.OnceAsync<I>();

			return ConvertToModelList(results);
		}

		private async Task<I> OnceSingle<I>(ChildQuery baseChildQuery)
		{
			I result;
			if (!string.IsNullOrEmpty(Token))
				result = await baseChildQuery.WithAuth(Token).OnceSingleAsync<I>();
			else
				result = await baseChildQuery.OnceSingleAsync<I>();

			return result;
		}

		private async Task<FirebaseObject<I>> Post<I>(ChildQuery baseFilterQuery, I model)
			where I : IModel
		{
			FirebaseObject<I> result = default(FirebaseObject<I>);

			if (!string.IsNullOrEmpty(Token))
				result = await baseFilterQuery.WithAuth(Token).PostAsync(model, false);
			else
				result = await baseFilterQuery.PostAsync(model, false);
			return result;
		}

		private async Task Put<I>(ChildQuery baseFilterQuery, I model)
		{
			if (!string.IsNullOrEmpty(Token))
				await baseFilterQuery.WithAuth(Token).PutAsync(model);
			else
				await baseFilterQuery.PutAsync(model);
		}

		private async Task Delete(ChildQuery baseFilterQuery)
		{
			if (!string.IsNullOrEmpty(Token))
				await baseFilterQuery.WithAuth(Token).DeleteAsync();
			else
				await baseFilterQuery.DeleteAsync();
		}

		private List<I> ConvertToModelList<I>(IReadOnlyCollection<FirebaseObject<I>> results)
			where I : IModel
		{
			var list = new List<I>();
			foreach (var result in results)
			{
				result.Object.ObjectId = result.Key;
				list.Add(result.Object);
			}
			return list;
		}

		private string FullResourcePath(string childResource)
		{
			string fullResourcePath = BaseResourcePath;
			if (!string.IsNullOrEmpty(childResource))
				fullResourcePath += childResource.StartsWith("/") ? childResource : "/" + childResource;
			return fullResourcePath;
		}

		#endregion

		#region CRUD methods

		#region Read Methods

		protected async Task<T> ReadSingle(string objectId, string childResource = null)
		{
			return await ReadSingle<T>(objectId, childResource);
		}

		protected async Task<I> ReadSingle<I>(string objectId, string childResource)
			where I : IModel, new()
		{
			if (string.IsNullOrEmpty(objectId))
				return new I();

			ChildQuery childQuery = BaseChildQuery(childResource).Child(objectId);
			return await OnceSingle<I>(childQuery);
		}

		protected async Task<List<T>> ReadLimitToFirst(int limit, string childResource = null)
		{
			return await ReadLimitToFirst<T>(limit, childResource);
		}

		protected async Task<List<I>> ReadLimitToFirst<I>(int limit, string childResource)
			where I : IModel
		{
			FilterQuery baseFilterQuery = BaseFilterQuery(childResource).LimitToFirst(limit);
			return await Once<I>(baseFilterQuery);
		}

		protected async Task<List<T>> ReadLimitToLast(int limit, string childResource = null)
		{
			return await ReadLimitToLast<T>(limit, childResource);
		}

		protected async Task<List<I>> ReadLimitToLast<I>(int limit, string childResource)
			where I : IModel
		{
			FilterQuery baseFilterQuery = BaseFilterQuery(childResource).LimitToLast(limit);
			return await Once<I>(baseFilterQuery);
		}

		protected async Task<List<T>> ReadPrev(int limit, string endAtId, string childResource = null)
		{
			return await ReadPrev<T>(limit, endAtId, childResource);
		}

		protected async Task<List<I>> ReadPrev<I>(int limit, string endAtId, string childResource)
			where I : IModel
		{
			OrderQuery baseFilterQuery = BaseFilterQuery(childResource);
			return await Once<I>(baseFilterQuery.EndAt(endAtId).LimitToLast(limit));
		}

		protected async Task<List<T>> ReadNext(int limit, string startAtId, string childResource = null)
		{
			return await ReadNext<T>(limit, startAtId, childResource);
		}

		protected async Task<List<I>> ReadNext<I>(int limit, string startAtId, string childResource)
			where I : IModel
		{
			OrderQuery baseFilterQuery = BaseFilterQuery(childResource);
			return await Once<I>(baseFilterQuery.StartAt(startAtId).LimitToFirst(limit));
		}

		#endregion

		#region Create Methods

		protected async Task<string> Create(T model, string childResource = null)
		{
			return await Create<T>(model, childResource);
		}

		protected async Task<string> Create<I>(I model, string childResource)
			where I : IModel
		{
			if (model.Equals(null))
				return null;

			DateTime currentTime = await TimeService.Instance.Now();
			model.CreatedAt = currentTime;
			model.UpdatedAt = currentTime;

			ChildQuery childQuery = BaseChildQuery(childResource);
			FirebaseObject<I> result = await Post(childQuery, model);

			return result.Key;
		}

		#endregion

		#region Update Methods

		protected async Task<bool> Update(T model, string childResource = null)
		{
			return await Update<T>(model, childResource);
		}

		protected async Task<bool> Update<I>(I model, string childResource)
			where I : IModel, new()
		{
			if (model.Equals(null) || string.IsNullOrEmpty(model.ObjectId))
				return false;

			I registeredModel = await ReadSingle<I>(model.ObjectId, childResource);

			if (registeredModel.Equals(null))
				return false;

			DateTime currentTime = await TimeService.Instance.Now();
			model.CreatedAt = registeredModel.CreatedAt;
			model.UpdatedAt = currentTime;

			ChildQuery childQuery = BaseChildQuery(childResource).Child(model.ObjectId);
			await Put(childQuery, model);

			return true;
		}

		#endregion

		#region Delete Methods

		protected async Task<bool> Delete(T model, string childResource = null)
		{
			return await Delete<T>(model, childResource);
		}

		protected async Task<bool> Delete<I>(I model, string childResource)
			where I : IModel
		{
			if (model.Equals(null) || string.IsNullOrEmpty(model.ObjectId))
				return false;

			ChildQuery childQuery = BaseChildQuery(childResource).Child(model.ObjectId);
			await Delete(childQuery);

			return true;
		}

		#endregion

		#endregion
	}
}
