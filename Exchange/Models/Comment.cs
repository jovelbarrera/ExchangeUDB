using System;
using Exchange.Interfaces;

namespace Exchange.Models
{
	public class Comment : IModel
	{
		public DateTimeOffset CreatedAt { get; set; }
		public string ObjectId { get; set; }
		public DateTimeOffset UpdatedAt { get; set; }
		public User User { get; set; }
		public string Message { get; set; }
	}
}

