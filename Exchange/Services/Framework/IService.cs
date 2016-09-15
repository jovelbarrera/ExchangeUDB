using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Kadevjo.Core.Models;

namespace Kadevjo.Core.Dependencies
{
	public interface IService<T>
	{
		Task<GenericResponse<U>> Create<U>(T entity);
		Task<GenericResponse<T>> Read(string id);
		Task<GenericResponse<List<T>>> ReadAll();
		Task<GenericResponse<List<T>>> ReadAll(IQuery query);
		Task<GenericResponse<U>> Update<U>(T entity);
		Task<GenericResponse<U>> Delete<U>(string id);

		// Generic GET, R = Response
		//Task<R> Execute<R> ( string resource, IQuery query );

		// Generic VERB, R = Request, B = Model
		//Task<R> Execute<R,B> ( string resource, Method method, B body );
	}
}
