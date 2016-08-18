using Exchange.Interfaces;
using Kadevjo.Core.Dependencies;
using Kadevjo.Core.Models;
using Kadevjo.Core.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Exchange.Services.FirebaseServices
{
    public abstract class FirebaseDatabaseService<S, T> : FlurlService<S, T>
        where S : IService<T>
        where T : Model, IModel, new()
    {
        protected abstract string Token { get; }

        #region CRUD methods

        #region Read Methods

        protected async Task<T> ReadSingle(string objectId)
        {
            return await ReadSingle<T>(objectId, Resource);
        }

        protected async Task<I> ReadSingle<I>(string objectId, string childResource)
            where I : IModel, new()
        {
            if (string.IsNullOrEmpty(objectId))
                return new I();

            string resource = string.Format("{0}/{1}.json?auth={2}", childResource, objectId, Token);
            I result = await Execute<I>(resource);
            if (result == null)
                return new I();
            result.ObjectId = objectId;
            return result;
        }

        protected async Task<List<I>> Read<I>(string childResource, Dictionary<string, object> parameters)
            where I : IModel
        {
            string parametersString = string.Empty;
            foreach (var parameter in parameters)
            {
                parametersString += string.Format("&{0}={1}", parameter.Key, parameter.Value);
            }
            string resource = string.Format("{0}.json?auth={1}{2}", childResource, Token, parametersString);
            Dictionary<string, I> results = await Execute<Dictionary<string, I>>(resource);
            if (results == null)
                return new List<I>();
            foreach (var result in results)
                result.Value.ObjectId = result.Key;

            List<I> items = results.Values.ToList();
            return items;
        }

        #endregion

        #region Create Methods

        protected async Task<string> Create(T model)
        {
            return await Create<T>(model, Resource);
        }

        protected async Task<string> Create<I>(I model, string childResource)
            where I : IModel
        {
            if (model.Equals(null))
                return null;

            DateTime currentTime = await TimeService.Instance.Now();
            model.CreatedAt = currentTime;
            model.UpdatedAt = currentTime;

            string resource = string.Format("{0}.json?auth={1}", childResource, Token);
            GenericResponse<Dictionary<string, string>> result = await Execute<Dictionary<string, string>, I>(resource, HttpMethod.Post, model);
            if (result != null && result.Model != null && result.Model.ContainsKey("name"))
                return result.Model["name"];
            return null;
        }

        #endregion

        #region Update Methods

        protected async Task<bool> Update(T model)
        {
            return await Update<T>(model, Resource);
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

            string resource = string.Format("{0}/{1}.json?auth={2}", childResource, model.ObjectId, Token);
            GenericResponse<I> result = await Execute<I, I>(resource, new HttpMethod("PATCH"), model);
            if (result != null && result.Model != null)
                return true;
            return false;
        }

        #endregion

        #region Delete Methods

        protected async Task<bool> Delete(T model)
        {
            return await Delete<T>(model, Resource);
        }

        protected async Task<bool> Delete<I>(I model, string childResource)
            where I : IModel
        {
            if (model.Equals(null) || string.IsNullOrEmpty(model.ObjectId))
                return false;

            string resource = string.Format("{0}/{1}.json?auth={2}", childResource, model.ObjectId, Token);
            GenericResponse<object> result = await Execute<object, I>(resource, HttpMethod.Delete);
            if (result.Status == System.Net.HttpStatusCode.OK)
                return true;
            return false;
        }

        #endregion

        #region Custom Methods

        protected async Task<List<T>> ReadLimitToFirst(int limit)
        {
            return await ReadLimitToFirst<T>(limit, Resource);
        }

        protected async Task<List<I>> ReadLimitToFirst<I>(int limit, string childResource)
            where I : IModel
        {
            var parameters = new Dictionary<string, object>
            {
                { "orderBy",@"""$key""" },
                { "limitToFirst" , limit }
            };

            return await Read<I>(childResource, parameters);
        }

        protected async Task<List<T>> ReadLimitToLast(int limit)
        {
            return await ReadLimitToLast<T>(limit, Resource);
        }

        protected async Task<List<I>> ReadLimitToLast<I>(int limit, string childResource)
            where I : IModel
        {
            var parameters = new Dictionary<string, object>
            {
                { "orderBy",@"""$key""" },
                { "limitToLast" , limit }
            };

            return await Read<I>(childResource, parameters);
        }

        protected async Task<List<T>> ReadPrev(int limit, string endAtId)
        {
            return await ReadPrev<T>(limit, endAtId, Resource);
        }

        protected async Task<List<I>> ReadPrev<I>(int limit, string endAtId, string childResource)
            where I : IModel
        {
            var parameters = new Dictionary<string, object>
            {
                { "orderBy",@"""$key""" },
                { "limitToFirst" , limit },
                { "EndAt" , endAtId }
            };
            return await Read<I>(childResource, parameters);
        }

        protected async Task<List<T>> ReadNext(int limit, string startAtId)
        {
            return await ReadNext<T>(limit, startAtId, Resource);
        }

        protected async Task<List<I>> ReadNext<I>(int limit, string startAtId, string childResource)
            where I : IModel
        {
            var parameters = new Dictionary<string, object>
            {
                { "orderBy",@"""$key""" },
                { "limitToFirst" , limit },
                { "StartAt" , startAtId }
            };
            return await Read<I>(childResource, parameters);
        }

        #endregion

        #endregion
    }
}
