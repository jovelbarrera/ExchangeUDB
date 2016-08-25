using System;
namespace Exchange.Interfaces
{
	public interface IUser : IModel
	{
		string Username { get; set; }
		string DisplayName { get; set; }
		string FirstName { get; set; }
		string LastName { get; set; }
		string Email { get; set; }
		string University { get; set; }
		string Career { get; set; }
		string About { get; set; }
		string ProfilePicture { get; set; }
	}
}

