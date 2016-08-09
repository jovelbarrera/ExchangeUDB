using System;
using Realms;

namespace Exchange.Interfaces
{
	public interface IModel
	{
		string ObjectId { get; set; }
		DateTimeOffset CreatedAt { get; set; }
		DateTimeOffset UpdatedAt { get; set; }
	}
}