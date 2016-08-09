using System;
using Realms;

namespace Exchange.RealmServices
{
	public class RealmService
	{
		private static Realm _instance;
		public static Realm Instance
		{
			get
			{
				if (_instance == null)
					_instance = Realm.GetInstance();
				return _instance;
			}
		}
	}
}

