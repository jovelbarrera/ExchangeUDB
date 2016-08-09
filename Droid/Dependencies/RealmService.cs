using System;
using System.Collections.Generic;
using System.Linq;
using Exchange.Droid.Dependencies;
using Realms;
using Xamarin.Forms;

[assembly: Dependency(typeof(RealmService))]
namespace Exchange.Droid.Dependencies
{
	public class RealmService
	{
		private Realm _realm;
		private RealmService _instance;
		public RealmService Instance
		{
			get
			{
				if (_instance == null)
					return new RealmService();
				return _instance;
			}
		}

		public RealmService()
		{
			_realm = Realm.GetInstance();
		}

		public List<T> Read<T>(Func<T, bool> whereQuery = null)
			where T : RealmObject, new()
		{
			IEnumerable<T> results;
			if (whereQuery == null)
				results = _realm.All<T>();
			else
				results = _realm.All<T>().Where(whereQuery);
			return results.ToList();
		}

		public T Create<T>(T entity)
			where T : RealmObject, new()
		{
			T realmEntity = null;
			_realm.Write(() =>
			{
				realmEntity = _realm.CreateObject<T>();
				realmEntity = entity;
			});
			return realmEntity;
		}

		//public void Update (T realmEntity)
		//{
		//	using (var trans = _realm.BeginWrite())
		//	{
		//		author.Name = "Thomas Pynchon";
		//		trans.Commit();
		//	}

		//}

		//public Delete()
		//{
		//}
	}
}

