using System;
using Exchange.Interfaces;

namespace Exchange.Models
{
	public class Exchange : IModel
	{
		public string ObjectId { get; set; }
		public DateTimeOffset CreatedAt { get; set; }
		public DateTimeOffset UpdatedAt { get; set; }

		public User User { get; set; }
		public string Title { get; set; }
		public string Description { get; set; }
		public string Thumbnail { get; set; }
	}
}

