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

			PrintHeader();
			Console.WriteLine("Press any key to start sending events");
			Console.ReadKey();

			// Create a producer client that you can use to send events to an event hub
			await using (var producerClient = new EventHubProducerClient(Settings.EventHubConnectionString, Settings.EventHubName))
			{
				// Create a batch of events
				using EventDataBatch eventDataBatch = await producerClient.CreateBatchAsync();

				// Add event to the batch. An event is represented by a collection of bytes and metadata.
				var order = new Order("TaleLearnCode", "Louisville", "KY");
				if (eventDataBatch.TryAdd(new EventData(Encoding.UTF8.GetBytes(JsonSerializer.Serialize(order)))))
					Console.WriteLine($"Added event for Order Id {order.Id}");

				await producerClient.SendAsync(eventDataBatch);

			}

			Console.WriteLine("All done");
			Console.ReadLine();

		}

		static void PrintHeader()
		{
			Console.ForegroundColor = ConsoleColor.Blue;
			Console.WriteLine(@"  _________                  .___ ___________                    __          ");
			Console.WriteLine(@" /   _____/ ____   ____    __| _/ \_   _____/__  __ ____   _____/  |_  ______");
			Console.WriteLine(@" \_____  \_/ __ \ /    \  / __ |   |    __)_\  \/ // __ \ /    \   __\/  ___/");
			Console.WriteLine(@" /        \  ___/|   |  \/ /_/ |   |        \\   /\  ___/|   |  \  |  \___ \ ");
			Console.WriteLine(@"/_______  /\___  >___|  /\____ |  /_______  / \_/  \___  >___|  /__| /____  >");
			Console.WriteLine(@"        \/     \/     \/      \/          \/           \/     \/          \/ ");
			Console.ResetColor();
			Console.WriteLine();
		}

	}

}