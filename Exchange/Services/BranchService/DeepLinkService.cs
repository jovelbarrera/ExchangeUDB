using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Exchange.Interfaces;

namespace Exchange.BranchService
{
	public abstract class DeepLinkService<I, T> : IDeepLinkService<I, T>
	where T : IModel
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

		public abstract void GenerateLink(Dictionary<string, object> data, IDeepLinkMetadata deepLinkMetadata);

		public abstract void ReadLink(Dictionary<string, object> data);

		protected T GetModel(Dictionary<string, object> data)
		{
			Type type = typeof(T);
			var model = Activator.CreateInstance(type);

			List<PropertyInfo> properties = type.GetRuntimeProperties().ToList();
			var propertiesNames = new List<string>();
			foreach (var property in properties)
				propertiesNames.Add(property.Name);

			foreach (var kv in data)
			{
				if (properties.Exists(i => i.Name == kv.Key))
					type.GetRuntimeProperty(kv.Key).SetValue(model, kv.Value);
			}
			return (T)model;
		}
	}
}

