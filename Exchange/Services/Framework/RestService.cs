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

		protected abstract Task<R> Execute<R>(string resource, IQuery query = null);

		protected abstract Task<bool> Execute<B>(string resource, HttpMethod method, B body = default(B));

		protected abstract Task<GenericResponse<R>> Execute<R, B>(string resource, HttpMethod method, B body = default(B));

		#region IService implementation

		public virtual async Task<GenericResponse<U>> Create<U>(T entity)
		{
			GenericResponse<U> response = await Execute<U,T>(Resource, HttpMethod.Post, entity);
			return response;
		}

		public virtual async Task<T> Read(string id)
		{
			string resource = Resource + "/" + id;

			if (isCacheable)
			{
				if (!CrossConnectivity.Current.IsConnected)
				{
					var response = await cacheManager.GetObject(id);
					return response;
				}
				else
				{
					var response = await Execute<T>(resource);
					await cacheManager.InsertObject(response);
					return response;
				}
			}
			else
			{
				return await Execute<T>(resource);
			}
		}

		public virtual async Task<List<T>> ReadAll()
		{
			if (isCacheable)
			{
				if (!CrossConnectivity.Current.IsConnected)
				{
					var response = await cacheManager.GetObjects();
					return response;
				}
				else
				{
					var response = await Execute<List<T>>(Resource);
					await cacheManager.InsertObjects(response);
					return response;
				}
			}
			else
			{
				return await Execute<List<T>>(Resource);
			}
		}

		public virtual async Task<List<T>> ReadAll(IQuery query)
		{
			if (isCacheable)
			{
				if (!CrossConnectivity.Current.IsConnected)
				{
					var response = await cacheManager.GetObjects();
					return response;
				}
				else
				{
					var response = await Execute<List<T>>(Resource, query);
					await cacheManager.InsertObjects(response);
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