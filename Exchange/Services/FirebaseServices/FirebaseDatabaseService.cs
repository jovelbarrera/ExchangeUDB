using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Exchange.Interfaces;
using Kadevjo.Core.Dependencies;
using Kadevjo.Core.Models;
using Kadevjo.Core.Services;

namespace Exchange.Services.FirebaseServices
{
	public abstract class FirebaseDatabaseService<S, T> : FlurlService<S, T>
		where S : IService<T>
		where T : Model, IModel, new()
	{
		protected abstract string Token { get; }

		#region CRUD methods

		#region Read Methods
		public override async Task<T> Read(string id)
		{
			return await Read<T>(id, Resource);
		}

		public override async Task<List<T>> ReadAll()
		{
			return await ReadAll<T>(Resource, null);
		}

		public override Task<List<T>> ReadAll(IQuery query)
		{
			return ReadAll<T>(Resource, query);
		}

		protected async Task<I> Read<I>(string objectId, string childResource)
			where I : IModel, new()
		{
			if (string.IsNullOrEmpty(objectId))
				return new I();

			var query = new FirebaseQuery();
			query.Auth(Token);
			string resource = string.Format("{0}/{1}.json", childResource, objectId, Token);
			I result = await Execute<I>(resource, query);
			if (result == null)
				return new I();
			result.ObjectId = objectId;
			return result;
		}

		protected async Task<List<I>> ReadAll<I>(string childResource, IQuery query = null)
			where I : IModel
		{
			var firebaseQuery = (FirebaseQuery)query;
			firebaseQuery.Auth(Token);

			string resource = string.Format("{0}.json", childResource);

			Dictionary<string, I> results = await Execute<Dictionary<string, I>>(resource, firebaseQuery);
			if (results == null)
				return new List<I>();
			foreach (var result in results)
				result.Value.ObjectId = result.Key;

			List<I> items = results.Values.ToList();
			return items;
		}

		#endregion

		#region Create Methods
		public override async Task<GenericResponse<U>> Create<U>(T entity)
		{
			return await Create<U, T>(entity, Resource);
		}

		protected async Task<GenericResponse<U>> Create<U, I>(I model, string childResource)
			where I : IModel
		{
			if (model == null)
				return null;

			DateTime currentTime = await TimeService.Instance.Now();
			model.CreatedAt = currentTime;
			model.UpdatedAt = currentTime;

			string resource = string.Format("{0}.json?auth={1}", childResource, Token);
			GenericResponse<U> result = await Execute<U, I>(resource, HttpMethod.Post, model);
			return result;
		}

		#endregion

		#region Update Methods
		public override async Task<GenericResponse<U>> Update<U>(T entity)
		{
			return await Update<U, T>(entity, Resource);
		}

		protected async Task<GenericResponse<U>> Update<U, I>(I model, string childResource)
			where I : IModel, new()
		{
			if (model.Equals(null) || string.IsNullOrEmpty(model.ObjectId))
				return null;

			I registeredModel = await Read<I>(model.ObjectId, childResource);

			if (registeredModel.Equals(null))
				return null;

			DateTime currentTime = await TimeService.Instance.Now();
			model.CreatedAt = registeredModel.CreatedAt;
			model.UpdatedAt = currentTime;

			string resource = string.Format("{0}/{1}.json?auth={2}", childResource, model.ObjectId, Token);
			GenericResponse<U> result = await Execute<U, I>(resource, new HttpMethod("PATCH"), model);
			return result;
		}
		#endregion

		#region Delete Methods

		public override async Task<GenericResponse<U>> Delete<U>(string id)
		{
			return await Delete<U, T>(id, Resource);
		}

		protected async Task<GenericResponse<U>> Delete<U, I>(string objectId, string childResource)
			where I : IModel
		{
			if (string.IsNullOrEmpty(objectId))
				return null;

			string resource = string.Format("{0}/{1}.json?auth={2}", childResource, objectId, Token);
			GenericResponse<U> result = await Execute<U, I>(resource, HttpMethod.Delete);
			return result;
		}
		#endregion

		#endregion
	}
}
