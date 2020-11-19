using Azure.Messaging.EventHubs;
using Azure.Messaging.EventHubs.Producer;
using SendAndReceiveEvents;
using System;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace SendEvents
{
	class Program
	{

		static async Task Main()
		{

			Console.WriteLine("Press any key to start sending events");
			Console.ReadKey();

			// Create a producer client that you can use to send events to an event hub
			await using (var producerClient = new EventHubProducerClient(Settings.EventHubConnectionString, Settings.EventHubName))
			{
				// Create a batch of events
				using EventDataBatch eventDataBatch = await producerClient.CreateBatchAsync();

				// Add event to the batch. An event is represented by a collection of bytes and metadata.
				//eventDataBatch.TryAdd(new EventData(Encoding.UTF8.GetBytes("First event")));
				//eventDataBatch.TryAdd(new EventData(Encoding.UTF8.GetBytes("Second event")));
				//eventDataBatch.TryAdd(new EventData(Encoding.UTF8.GetBytes("Third event")));

				var order = new Order("TaleLearnCode", "Louisville", "KY");

				if (eventDataBatch.TryAdd(new EventData(Encoding.UTF8.GetBytes(JsonSerializer.Serialize(order)))))
					Console.WriteLine($"Added event for Order Id {order.Id}");

				await producerClient.SendAsync(eventDataBatch);
				//Console.WriteLine("A batch of 3 events has been published.");

			}

			Console.WriteLine("All done");
			Console.ReadLine();

		}
	}
}