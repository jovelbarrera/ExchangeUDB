using System;
using System.Collections.Generic;
using Exchange.Interfaces;
using Newtonsoft.Json;

namespace Exchange.Models
{
	public class Ask : IModel
	{
		public string ObjectId { get; set; }
		public DateTimeOffset CreatedAt { get; set; }
		public DateTimeOffset UpdatedAt { get; set; }

		public User User { get; set; }
		public string Title { get; set; }
		public string Description { get; set; }
		public string[] Tags { get; set; }
	}
}

