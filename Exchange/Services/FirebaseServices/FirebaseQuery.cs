using System;
using System.Collections.Generic;
using Kadevjo.Core.Dependencies;

namespace Exchange.Services.FirebaseServices
{
	public class FirebaseQuery : IQuery
	{
		private Dictionary<string, object> parameters;

		#region IQuery implementation

		public Dictionary<string, object> Parameters { get { return parameters; } }

		public Dictionary<string, object> FormattedParameters { get { return Parameters; } }

		public FirebaseQuery()
		{
			parameters = new Dictionary<string, object>();
		}

		public IQuery Add(IQuery query)
		{
			foreach (var kvp in query.Parameters)
			{
				parameters.Add(kvp.Key, kvp.Value);
			}

			return this;
		}

		public IQuery Add(string property, object value)
		{
			if (parameters.ContainsKey(property))
			{
				parameters[property] = value;
			}
			else {
				parameters.Add(property, value);
			}

			return this;
		}

		#region Compatibles
		public IQuery Equal(string property, object value)
		{
			Add("equalTo", value);
			return this;
		}

		// OK
		public IQuery OrderBy(string property)
		{
			Add("orderBy", string.Format(@"""{0}""", property));
			return this;
		}

		#endregion

		#region Not compatible

		public IQuery NotEqual(string property, object value)
		{
			throw new NotImplementedException();
		}

		public IQuery GreaterThan(string property, object value)
		{
			throw new NotImplementedException();
		}

		public IQuery GreaterThanOrEqual(string property, object value)
		{
			throw new NotImplementedException();
		}

		public IQuery LowerThan(string property, object value)
		{
			throw new NotImplementedException();
		}

		public IQuery LowerThanOrEqual(string property, object value)
		{
			throw new NotImplementedException();
		}

		public IQuery ContainedIn(string property, object value)
		{
			throw new NotImplementedException();
		}

		public IQuery NotContainedIn(string property, object value)
		{
			throw new NotImplementedException();
		}

		public IQuery Limit(int value)
		{
			throw new NotImplementedException();
		}

		public IQuery Skip(int value)
		{
			throw new NotImplementedException();
		}

		public IQuery OrderByDescending(string property)
		{
			throw new NotImplementedException();
		}

		#endregion

		#endregion

		public IQuery Auth(string token)
		{
			Add("auth", token);
			return this;
		}

		public IQuery StartAt(string id)
		{
			Add("startAt", string.Format(@"""{0}""", id));
			return this;
		}

		public IQuery EndAt(string id)
		{
			Add("endAt", string.Format(@"""{0}""", id));
			return this;
		}

		public IQuery LimitToFirst(int value)
		{
			Add("limitToFirst", value);
			return this;
		}

		public IQuery LimitToLast(int value)
		{
			Add("limitToLast", value);
			return this;
		}
	}
}