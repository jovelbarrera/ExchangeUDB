using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Exchange.Interfaces;
using Exchange.Models;
using Exchange.RealmServices;
using Exchange.Services.AuthService;
using Exchange.Services.FirebaseServices;
using Kadevjo.Core.Dependencies;
using Kadevjo.Core.Models;
using Exchange.Helpers;
using System;

namespace Exchange.Services
{
	public class CustomUserManager : UserManager<CustomUserManager, PersistentUser>
	{
		public override ICache<PersistentUser> CacheService
		{
			get
			{
				return RealmService<PersistentUser>.Instance;
			}
		}

		public override async Task<PersistentUser> GetCurrentUser()
		{
			IUser user = await base.GetCurrentUser();
			if (user != null)
			{
				GenericResponse<User> result = await FirebaseUserService.Instance.Read(user.ObjectId);
				user = result.Model;
			}
			return new PersistentUser(user);
		}

		public override async Task SetCurrentUser(PersistentUser user)
		{
			IUser registeredUser = await FirebaseUserService.Instance.SecureUserRegister(new User(user));
			await base.SetCurrentUser(new PersistentUser(registeredUser));
		}

		public override async Task UpdateCurrentUser(PersistentUser user)
		{
			GenericResponse<Dictionary<string, string>> response = await FirebaseUserService.Instance.Update<Dictionary<string, string>>(new User(user));
			await base.UpdateCurrentUser(new PersistentUser(user));
		}

        public override Task<bool> DeleteCurrentUser()
        {
            Settings.FirebaseUserToken = string.Empty;
            Settings.FirebaseUserRefreshToken = string.Empty;
            Settings.FirebaseUserTokenExpiration = default(DateTime);
            return base.DeleteCurrentUser();
        }
    }
}

