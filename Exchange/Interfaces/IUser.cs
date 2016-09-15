using System;
using System.Collections.Generic;

namespace Exchange.Interfaces
{
	public interface IUser : IModel
	{
		string Username { get; set; }
		string DisplayName { get; set; }
		string FirstName { get; set; }
		string LastName { get; set; }
		string Email { get; set; }
		string ProfilePicture { get; set; }
		Dictionary<string, object> Data { get; set; }
	}
}

