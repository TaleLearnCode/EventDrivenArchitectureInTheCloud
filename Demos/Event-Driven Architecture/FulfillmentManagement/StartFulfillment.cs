using Azure.Messaging.EventHubs;
using Azure.Messaging.EventHubs.Consumer;
using Azure.Messaging.EventHubs.Processor;
using Azure.Storage.Blobs;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Text;
using System.Threading.Tasks;
using TaleLearnCode.EventDrivenArchitectureInTheCloud;

namespace FulfillmentManagement
{
	public static class StartFulfillment
	{

		static ILogger logger;

		[FunctionName("StartFulfillment")]
		public static async Task RunAsync(
			[TimerTrigger("0 */2 * * * *")] TimerInfo myTimer,
			ILogger log)
		{

			log.LogError("Fulfillment Processing starting");


			logger = log;

			// Read from the default consumer group: $Default
			string consumerGroup = EventHubConsumerClient.DefaultConsumerGroupName;

			// Create a blob container client that the event processor will use 
			BlobContainerClient storageClient = new BlobContainerClient(
				Environment.GetEnvironmentVariable("BlobStorageConnectionString"),
				Environment.GetEnvironmentVariable("BlobContainerName"));

			// Create an event processor client to process events in the event hub
			EventProcessorClient processor = new EventProcessorClient(
				storageClient, consumerGroup,
				Environment.GetEnvironmentVariable("EventHubConnectionString"),
				Environment.GetEnvironmentVariable("EventHubName"));

			// Register handlers for processing events and handling errors
			processor.ProcessEventAsync += ProcessEventHandler;
			processor.ProcessErrorAsync += ProcessErrorHandler;

			// Start the processing
			await processor.StartProcessingAsync();

			// Wait for 10 seconds for the events to be processed
			await Task.Delay(TimeSpan.FromSeconds(10));

			// Stop the processing
			await processor.StopProcessingAsync();

		}

		static async Task ProcessEventHandler(ProcessEventArgs eventArgs)
		{
			// Write the body of the event to the console window
			//Console.WriteLine("\tReceived event: {0}", Encoding.UTF8.GetString(eventArgs.Data.Body.ToArray()));
			var messageBody = Encoding.UTF8.GetString(eventArgs.Data.Body.ToArray());
			Order order = JsonConvert.DeserializeObject<Order>(messageBody);

			// Here we would start the fulfillment process
			logger.LogWarning($"Shipping order to : {order.ShippingAddress.StreetAddress}, {order.ShippingAddress.City}, {order.ShippingAddress.State} {order.ShippingAddress.PostalCode}");

			// Update checkpoint in the blob storage so that the app receives only new events the next time it's run
			await eventArgs.UpdateCheckpointAsync(eventArgs.CancellationToken);
		}

		static Task ProcessErrorHandler(ProcessErrorEventArgs eventArgs)
		{
			// Write details about the error to the console window
			Console.WriteLine($"\tPartition '{ eventArgs.PartitionId}': an unhandled exception was encountered. This was not expected to happen.");
			Console.WriteLine(eventArgs.Exception.Message);
			return Task.CompletedTask;
		}

	}
}