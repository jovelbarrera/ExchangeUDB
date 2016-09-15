using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Reflection;
using System.Threading.Tasks;
using Kadevjo.Core.Attributes;
using Kadevjo.Core.Dependencies;
using Kadevjo.Core.Models;
using Plugin.Connectivity;

namespace Kadevjo.Core.Services
{
	public abstract class RestService<I, T> : IService<T>
	{
		private static I instance;
		public static I Instance
		{
			get
			{
				if (instance == null)
				{
					instance = (I)Activator.CreateInstance(typeof(I));
				}

				return instance;
			}
		}

		private bool isCacheable = false;
		private ICache<T> cacheManager; // Using new ICache Interface

		public RestService()
		{
			var attrs = typeof(I).GetTypeInfo().GetCustomAttributes();
			foreach (var attr in attrs)
			{
				if (attr is CacheableAttribute)
				{
					isCacheable = true;
				}
			}
		}

		protected abstract string BaseUrl { get; }

		protected abstract Dictionary<string, string> Headers { get; }

		protected abstract string Resource { get; }

		protected abstract Task<GenericResponse<R>> Execute<R>(string resource, IQuery query = null);

		protected abstract Task<bool> Execute<B>(string resource, HttpMethod method, B body = default(B));

		protected abstract Task<GenericResponse<R>> Execute<R, B>(string resource, HttpMethod method, B body = default(B), IQuery query = null);

		#region IService implementation

		public virtual async Task<GenericResponse<U>> Create<U>(T entity)
		{
			GenericResponse<U> response = await Execute<U,T>(Resource, HttpMethod.Post, entity);
			return response;
		}

		public virtual async Task<GenericResponse<T>> Read(string id)
		{
			string resource = Resource + "/" + id;

			if (isCacheable)
			{
				if (!CrossConnectivity.Current.IsConnected)
				{
					T response = await cacheManager.GetObject(id);
					return new GenericResponse<T> { Model = response };
				}
				else
				{
					GenericResponse<T> response = await Execute<T>(resource);
					await cacheManager.InsertObject(response.Model);
					return response;
				}
			}
			else
			{
				GenericResponse<T> response = await Execute<T>(resource);
				return response;
			}
		}

		public virtual async Task<GenericResponse<List<T>>> ReadAll()
		{
			if (isCacheable)
			{
				if (!CrossConnectivity.Current.IsConnected)
				{
					List<T> response = await cacheManager.GetObjects();
					return new GenericResponse<List<T>> { Model = response };
				}
				else
				{
					GenericResponse<List<T>> response = await Execute<List<T>>(Resource);
					await cacheManager.InsertObjects(response.Model);
					return response;
				}
			}
			else
			{
				GenericResponse<List<T>> response = await Execute<List<T>>(Resource);
				return response;
			}
		}

		public virtual async Task<GenericResponse<List<T>>> ReadAll(IQuery query)
		{
			if (isCacheable)
			{
				if (!CrossConnectivity.Current.IsConnected)
				{
					List<T> response = await cacheManager.GetObjects();
					return new GenericResponse<List<T>> { Model = response };
				}
				else
				{
					GenericResponse<List<T>> response = await Execute<List<T>>(Resource, query);
					await cacheManager.InsertObjects(response.Model);
					return response;
				}
			}
			else
			{
				return await Execute<List<T>>(Resource, query);
			}
		}

		public virtual async Task<GenericResponse<U>> Update<U>(T entity)
		{
			GenericResponse<U> response = await Execute<U, T>(Resource, HttpMethod.Put, entity);
			return response;
		}

		public virtual async Task<GenericResponse<U>> Delete<U>(string id)
		{
			string resource = Resource + "/" + id;
			GenericResponse<U> response = await Execute<U, T>(resource, HttpMethod.Delete);
			return response;
		}

		#endregion
	}
}