using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Exchange.Configs;
using Exchange.Helpers;
using Exchange.Interfaces;
using Exchange.Models;
using Kadevjo.Core.Models;

namespace Exchange.Services.FirebaseServices
{
	public class FirebaseUserService : FirebaseDatabaseService<FirebaseUserService, User>
	{
		protected override string Resource { get { return "User"; } }

		protected override string Token { get { return Settings.FirebaseUserToken; } }

		protected override string BaseUrl { get { return FirebaseAccess.Instance.FirebaseBasePath; } }

		protected override Dictionary<string, string> Headers { get { return new Dictionary<string, string>(); } }

		public async Task<IUser> SecureUserRegister(IUser user)
		{
			GenericResponse<User> result =await Read(user.ObjectId);
			User registeredUser = result.Model;

			if (registeredUser == null || string.IsNullOrEmpty(registeredUser.ObjectId))
			{
				user.CreatedAt = await TimeService.Instance.Now();
				GenericResponse<Dictionary<string, string>> response = await Update<Dictionary<string, string>>(new User(user));
				return user;
			}
			return registeredUser;
		}

		public Task SecureUserDelete(IUser user)
		{
			throw new NotImplementedException();
		}
	}
}

