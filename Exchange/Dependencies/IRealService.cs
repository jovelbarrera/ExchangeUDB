using System;
using System.Collections.Generic;

namespace Exchange.Dependencies
{
	public interface IRealService
	{
		List<T> Read<T>(Func<T, bool> whereQuery = null);
		T Create<T>(T entity);
	}
}

