using System;
using System.Collections.Generic;
using Exchange.Interfaces;
using Microsoft.WindowsAzure.MobileServices;
using Newtonsoft.Json;

namespace Exchange.Models
{
	[DataTable("Questions")]
	public class Question : IModel
	{
		[JsonProperty("id")]
		public string ObjectId { get; set; }
		[JsonProperty("createdAt")]
		public DateTimeOffset CreatedAt { get; set; }
		[JsonProperty("updatedAt")]
		public DateTimeOffset UpdatedAt { get; set; }

		[JsonProperty("userId")]
		public string UserId { get; set; }
		[JsonIgnore]
		public AppUser User { get; set; }
		[JsonProperty("title")]
		public string Title { get; set; }
		[JsonProperty("content")]
		public string Description { get; set; }
	}
}

