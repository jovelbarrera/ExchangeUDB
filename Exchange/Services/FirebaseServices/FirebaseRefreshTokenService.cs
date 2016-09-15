using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Exchange.Configs;
using Exchange.Helpers;
using Exchange.Services.FirebaseServices;
using Kadevjo.Core.Models;
using Kadevjo.Core.Services;

namespace Exchange.Services.FirebaseServices
{
	public class FirebaseRefreshTokenService : FlurlService<FirebaseRefreshTokenService, FirebaseToken>
	{
		protected override string BaseUrl
		{
			get { return FirebaseAccess.Instance.FirebaseBaseRefreshTokenPath; }
		}

		protected override Dictionary<string, string> Headers
		{
			get { return new Dictionary<string, string>(); }
		}

		protected override string Resource
		{
			get { return "token"; }
		}

		public async Task<FirebaseToken> Refresh(string apikey, string refreshToken)
		{
			var query = new FirebaseQuery();
			query.Add("key", apikey);
			//string resource = string.Format("?key={0}", apikey);
			var data = new Dictionary<string, object>
			{
				{"grant_type","refresh_token"},
				{"refresh_token",refreshToken},
			};
			GenericResponse<FirebaseToken> result = await Execute<FirebaseToken, Dictionary<string, object>>(Resource, HttpMethod.Post, data, query);
			return result.Model;
		}
	}
}

