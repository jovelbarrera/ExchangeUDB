using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Exchange.Interfaces;
using Kadevjo.Core.Dependencies;

namespace Exchange.Services.AuthService
{
    public abstract class UserManager<I, T> : IUserManager<T>
        where T : IUser
    {
        public abstract ICache<T> CacheService { get; }

        private static I _instance;
        public static I Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = (I)Activator.CreateInstance(typeof(I));
                }

                return _instance;
            }
        }

        public virtual async Task<T> GetCurrentUser()
        {
            List<T> users = (await CacheService.GetObjects()).ToList();
            T user = users.FirstOrDefault();
            return user;
        }

        public virtual async Task SetCurrentUser(T user)
        {
            T currentUser = await GetCurrentUser();

            if (currentUser == null || string.IsNullOrEmpty(currentUser.ObjectId))
                await CacheService.InsertObject(user);
            else
                await CacheService.UpdateObject(user);
        }

        public virtual async Task UpdateCurrentUser(T user)
        {
            T currentUser = await GetCurrentUser();
            await CacheService.UpdateObject(user);
        }

        public virtual async Task<bool> DeleteCurrentUser()
        {
            try
            {
                await CacheService.RemoveObjects();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}

