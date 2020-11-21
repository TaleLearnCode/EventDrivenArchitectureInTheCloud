using Microsoft.Azure.EventHubs;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaleLearnCode.EventDrivenArchitectureInTheCloud;

namespace InventoryManagement
{
	public static class UpdateInventory
	{
		[FunctionName("UpdateInventory")]
		public static async Task Run([EventHubTrigger("OrderProcessing", Connection = "EventHubConnectionString", ConsumerGroup = "inventory")] EventData[] events, ILogger log)
		{
			var exceptions = new List<Exception>();

			foreach (EventData eventData in events)
			{
				try
				{
					string messageBody = Encoding.UTF8.GetString(eventData.Body.Array, eventData.Body.Offset, eventData.Body.Count);
					Order order = JsonConvert.DeserializeObject<Order>(messageBody);

					// Here we would do what we need to manage inventory
					log.LogInformation($"Taking a '{order.OrderItem}' out of availablle inventory");
					await Task.Yield();
				}
				catch (Exception e)
				{
					// We need to keep processing the rest of the batch - capture this exception and continue.
					// Also, consider capturing details of the message that failed processing so it can be processed again later.
					exceptions.Add(e);
				}
			}

			// Once processing of the batch is complete, if any messages in the batch failed processing throw an exception so that there is a record of the failure.

			if (exceptions.Count > 1)
				throw new AggregateException(exceptions);

			if (exceptions.Count == 1)
				throw exceptions.Single();
		}
	}
}
