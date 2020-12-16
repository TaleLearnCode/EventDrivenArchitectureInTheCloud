using System;

namespace SendAndReceiveEvents
{
	public record Order(string CustomerName, string	City, string State)
	{
		public string Id { get; init; } = Guid.NewGuid().ToString();
	}
}