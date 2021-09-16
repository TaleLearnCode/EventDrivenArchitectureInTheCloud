using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.IO;
using System.Threading.Tasks;
using TaleLearnCode.EventDrivenArchitectureInTheCloud;

namespace OrderProcessing
{
	public static class CreateOrder
	{

		[FunctionName("CreateOrder")]
		public static async Task<IActionResult> Run(
				[HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest request,
				[EventHub(
					"OrderProcessing",
					Connection = "EventHubConnectionString")] IAsyncCollector<string> outputEvents,
				ILogger log)
		{

			// Get the order details
			string requestBody = await new StreamReader(request.Body).ReadToEndAsync();
			Order order = JsonConvert.DeserializeObject<Order>(requestBody);
			if (order is null) return new BadRequestObjectResult("No order details supplied");

			// Do some fancy order processing logic
			log.LogInformation($"Processing order #{order.Id}");
			log.LogWarning("Credit card processed");

			// Publish the payment event
			await outputEvents.AddAsync(JsonConvert.SerializeObject(order));

			// Return a confirmation
			return new OkObjectResult($"Order #{order.Id} is now being processed.");

		}
	}
}