using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Exchange.Configs;
using Exchange.Helpers;
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
		public override async Task<GenericResponse<T>> Read(string id)
		{
			return await Read<T>(id, Resource);
		}

		public override async Task<GenericResponse<List<T>>> ReadAll()
		{
			return await ReadAll<T>(Resource, null);
		}

		public override Task<GenericResponse<List<T>>> ReadAll(IQuery query)
		{
			return ReadAll<T>(Resource, query);
		}

		protected async Task<GenericResponse<I>> Read<I>(string objectId, string childResource)
			where I : IModel, new()
		{
			if (string.IsNullOrEmpty(objectId))
				return new GenericResponse<I>();

			var query = new FirebaseQuery();
			query.Auth(Token);
			string resource = string.Format("{0}/{1}.json", childResource, objectId, Token);
			GenericResponse<I> result = await EnsureAuthorizedRequest(Execute<I>(resource, query));
			if (result == null || result.Model == null)
				return new GenericResponse<I>();
			result.Model.ObjectId = objectId;
			return result;
		}

		protected async Task<GenericResponse<List<I>>> ReadAll<I>(string childResource, IQuery query = null)
			where I : IModel
		{
			var firebaseQuery = (FirebaseQuery)query;
			firebaseQuery.Auth(Token);

			string resource = string.Format("{0}.json", childResource);

			GenericResponse<Dictionary<string, I>> result = await EnsureAuthorizedRequest(Execute<Dictionary<string, I>>(resource, firebaseQuery));
			if (result == null || result.Model == null)
				return new GenericResponse<List<I>>();
			foreach (var r in result.Model)
				r.Value.ObjectId = r.Key;

			List<I> items = result.Model.Values.ToList();
			return new GenericResponse<List<I>> { Status = result.Status, Model = items };
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
			GenericResponse<U> result = await EnsureAuthorizedRequest(Execute<U, I>(resource, HttpMethod.Post, model));
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
			if (model == null || string.IsNullOrEmpty(model.ObjectId))
				return null;

			GenericResponse<I> readResult = await Read<I>(model.ObjectId, childResource);
			I registeredModel = readResult.Model;

			DateTime currentTime = await TimeService.Instance.Now();
			if (registeredModel == null || string.IsNullOrEmpty(registeredModel.ObjectId))
				model.UpdatedAt = currentTime;
			else
				model.CreatedAt = registeredModel.CreatedAt;
			model.UpdatedAt = currentTime;

			string resource = string.Format("{0}/{1}.json?auth={2}", childResource, model.ObjectId, Token);
			GenericResponse<U> result = await EnsureAuthorizedRequest(Execute<U, I>(resource, new HttpMethod("PATCH"), model));
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
			GenericResponse<U> result = await EnsureAuthorizedRequest(Execute<U, I>(resource, HttpMethod.Delete));
			return result;
		}
		#endregion

		#endregion

		#region Token methods
		private async Task<GenericResponse<U>> EnsureAuthorizedRequest<U>(Task<GenericResponse<U>> service)
		{
			if (service == null)
				return default(GenericResponse<U>);
			await service.ConfigureAwait(false);
			GenericResponse<U> response = service.Result;
			if (response.Status == null || response.Status.StatusCode == System.Net.HttpStatusCode.Unauthorized)
			{
				FirebaseToken token = await FirebaseRefreshTokenService.Instance.Refresh(FirebaseAccess.Instance.ApiKey, Settings.FirebaseUserRefreshToken);
				if (token != null)
				{
					Settings.FirebaseUserRefreshToken = token.RefreshToken;
					Settings.FirebaseUserToken = token.AccessToken;

					await service.ConfigureAwait(false);
					response = service.Result;
				}
			}
			return response;
		}
		#endregion
	}
}
