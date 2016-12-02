using System;
using Exchange.Interfaces;
using Microsoft.WindowsAzure.MobileServices;
using Newtonsoft.Json;
using Realms;

namespace Exchange.Models
{
	[DataTable("Answers")]
	public class Answer
	{
		[JsonProperty("createdAt")]
		public DateTimeOffset CreatedAt { get; set; }
		[JsonProperty("id")]
		public string ObjectId { get; set; }
		[JsonProperty("updatedAt")]
		public DateTimeOffset UpdatedAt { get; set; }
		[Ignored]
		public AppUser User { get; set; }

		[JsonProperty("userId")]
		public string UserId { get; set; }
		[JsonProperty("questionId")]
		public string QuestionId { get; set; }
		[JsonProperty("content")]
		public string Message { get; set; }
	}
}

