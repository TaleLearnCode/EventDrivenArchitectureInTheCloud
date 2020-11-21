using Microsoft.Azure.EventHubs;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TaleLearnCode.EventDrivenArchitectureInTheCloud;

namespace NotificationManager
{
	public static class ConfirmOrder
	{
		[FunctionName("ConfirmOrder")]
		public static async Task Run([EventHubTrigger("OrderProcessing", Connection = "EventHubConnectionString", ConsumerGroup = "notification")] EventData[] events, ILogger log)
		{
			var exceptions = new List<Exception>();

			foreach (EventData eventData in events)
			{
				try
				{
					string messageBody = Encoding.UTF8.GetString(eventData.Body.Array, eventData.Body.Offset, eventData.Body.Count);
					Order order = JsonConvert.DeserializeObject<Order>(messageBody);

					// Replace these two lines with your processing logic.
					log.LogWarning($"Sending email to {order.Customer.EmailAddress} confirming their order");
					Thread.Sleep(7500);
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
