using System;
namespace Exchange.Models
{
	public enum ActivityType
	{
		Undefined,
		Ask,
		Response,
		Comment,
	}

	public class Activity
	{
		public string ObjectId { get; set; }
		public User User { get; set; }
		public string Detail { get; set; }
		public DateTime Time { get; set; }
		public ActivityType Type { get; set; }
		public string ResourceId { get; set; }

		public string TypeString()
		{
			switch (Type)
			{
				case ActivityType.Ask:
					return "Ask";
				case ActivityType.Response:
					return "Respuesta";
				case ActivityType.Comment:
					return "Comentario";
				case ActivityType.Undefined:
				default:
					return "";
			}
		}
	}
}

