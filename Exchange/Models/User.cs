﻿using System;
using System.Collections.Generic;
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
		public string ProfilePicture { get; set; }
		public Dictionary<string, object> Data { get; set; }
		#endregion

		public User()
		{
			Data = new Dictionary<string, object>();
		}

		public User(IUser user)
		{
			if (user == null)
				return;
			ObjectId = user.ObjectId;
			CreatedAt = user.CreatedAt;
			UpdatedAt = user.UpdatedAt;
			Username = user.Username;
			DisplayName = user.DisplayName;
			FirstName = user.FirstName;
			LastName = user.LastName;
			Email = user.Email;
			ProfilePicture = user.ProfilePicture;
			Data = user.Data;
		}

		public object GetData(string key)
		{
			if (Data != null && Data.ContainsKey(key))
				return Data[key];
			return null;
		}
	}
}

