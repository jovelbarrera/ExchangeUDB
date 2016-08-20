using System;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Kadevjo.Core.Dependencies
{
    // Proposed ICache Interface
    public interface ICache<T>
    {
        string DBName { get; }
        Task<List<T>> GetObjects();
        Task<T> GetObject(string objectId);
        Task<List<T>> GetObjects(Func<T, bool> whereQuery);
        Task InsertObjects(List<T> objects);
        Task InsertObject(T obj);
        Task UpdateObject(T obj);
        Task RemoveObjects();
        Task RemoveObject(string objectId);
    }

    // Old ICache Interface
    //public interface ICache
    //{
    //    string AppName { get; set; }
    //    Task<List<T>> GetObjects<T>();
    //    Task<T> GetObject<T>(string key);
    //    Task InsertObjects<T>(List<T> objects);
    //    Task InsertObject<T>(T value);
    //    Task RemoveObjects<T>();
    //    Task RemoveObject<T>(string key);
    //    Task<string> LoadImage(string url);
    //}
}