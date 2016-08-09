using System;
namespace Exchange.Models
{
	public enum NotificationType
	{
		Undefined,
		NewExchange,
		AskActivity,
		Feed,
	}

	public class Notification
	{
		public string ObjectId { get; set; }
		public User User { get; set; }
		public string Detail { get; set; }
		public DateTime Time { get; set; }
		public NotificationType Type { get; set; }
		public string ResourceId { get; set; }
	}
}

