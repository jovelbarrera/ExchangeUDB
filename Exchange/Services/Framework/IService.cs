using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Kadevjo.Core.Dependencies
{
    public interface IService<T>
    {
        Task<bool> Create(T entity);
        Task<T> Read(string id);
        Task<List<T>> ReadAll();
        Task<List<T>> ReadAll(IQuery query);
        Task<bool> Update(T entity);
        Task<bool> Delete(string id);

        // Generic GET, R = Response
        //Task<R> Execute<R> ( string resource, IQuery query );

        // Generic VERB, R = Request, B = Model
        //Task<R> Execute<R,B> ( string resource, Method method, B body );
    }
}
