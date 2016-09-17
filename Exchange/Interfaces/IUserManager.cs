using System;
using System.Threading.Tasks;
using Kadevjo.Core.Dependencies;
using Kadevjo.Core.Models;

namespace Exchange.Interfaces
{
	public interface IUserManager<T>
		where T : IUser
	{
		ICache<T> CacheService { get; }
		Task<T> GetCurrentUser();
		Task SetCurrentUser(T user);
		Task UpdateCurrentUser(T user);
        Task<bool> DeleteCurrentUser();
    }
}

