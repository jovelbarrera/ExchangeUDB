using System;
using Exchange.Interfaces;
using Kadevjo.Core.Models;
using Newtonsoft.Json;

namespace Exchange.Services.FirebaseServices
{
	public class FirebaseToken : Model, IModel
	{
		[JsonProperty("access_token")]
		public string AccessToken { get; set; }
		[JsonProperty("expires_in")]
		public string ExpiresIn { get; set; }
		[JsonProperty("token_type")]
		public string TokenType { get; set; }
		[JsonProperty("refresh_token")]
		public string RefreshToken { get; set; }
		[JsonProperty("id_token")]
		public string ObjectId { get; set; }
		[JsonProperty("user_id")]
		public string UserId { get; set; }
		[JsonProperty("project_id")]
		public string ProjectId { get; set; }

		[JsonIgnore]
		public DateTimeOffset CreatedAt { get; set; }
		[JsonIgnore]
		public DateTimeOffset UpdatedAt { get; set; }
	}
}

