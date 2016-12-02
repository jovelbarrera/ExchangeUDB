using System;
using System.Collections.Generic;
using Exchange.Interfaces;
using Microsoft.WindowsAzure.MobileServices;
using Newtonsoft.Json;
using Realms;

namespace Exchange.Models
{
	[DataTable("Users")]
	public class AppUser
	{
		[JsonProperty("id")]
		public string ObjectId { get; set; }
		[JsonProperty("createdAt")]
		public DateTimeOffset CreatedAt { get; set; }
		[JsonProperty("updatedAt")]
		public DateTimeOffset UpdatedAt { get; set; }

		[JsonProperty("name")]
		public string Name { get; set; }
		[JsonProperty("picture")]
		public string ProfilePicture { get; set; }

	}
}

