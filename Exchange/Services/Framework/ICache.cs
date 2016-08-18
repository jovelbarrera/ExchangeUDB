using System;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Kadevjo.Core.Dependencies
{
    public interface ICache
    {
        string AppName { get; set; }
        Task<List<T>> GetObjects<T>();
        Task<T> GetObject<T>(string key);
        Task InsertObjects<T>(List<T> objects);
        Task InsertObject<T>(T value);
        Task RemoveObjects<T>();
        Task RemoveObject<T>(string key);
        Task<string> LoadImage(string url);
    }
}