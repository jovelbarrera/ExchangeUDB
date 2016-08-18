using System;
using Exchange.Interfaces;
using Realms;

namespace Exchange.Models
{
	public class User : Kadevjo.Core.Models.Model, IUser // Shouldn't inherit Model
    {
		#region IModel implementation
		public string ObjectId { get; set; }
		public DateTimeOffset CreatedAt { get; set; }
		public DateTimeOffset UpdatedAt { get; set; }
		#endregion

		#region IUser implementation
		public string Username { get; set; }
		public string DisplayName { get; set; }
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public string Email { get; set; }
		public string University { get; set; }
		public string Career { get; set; }
		public string About { get; set; }
		public string ProfilePicture { get; set; }
		#endregion
	}
}

