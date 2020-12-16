using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using TaleLearnCode.EventDrivenArchitectureInTheCloud;

namespace OrderSimulator
{
	class Program
	{

		static readonly HttpClient httpClient = new HttpClient();


		static async Task Main()
		{

			WelcomeUser();

			Console.WriteLine("Press any key to start simulating orders...");

			Console.ReadKey();

			int orderCounter = 0;
			Console.WriteLine();
			do
			{
				while (!Console.KeyAvailable)
				{
					var order = BogusRepository.GetOrder();
					StringContent body = new StringContent(JsonSerializer.Serialize(order));
					await httpClient.PostAsync("http://localhost:5860/api/CreateOrder", body);
					Console.WriteLine($"Sent order '{order.Id}' to the order processor");
					orderCounter++;
					await Task.Delay(500);
				}
			} while (Console.ReadKey(true).Key != ConsoleKey.Escape);

			Console.Clear();
			Console.WriteLine($"Simulated {orderCounter} orders");
			Console.ReadLine();

		}

		private static void WelcomeUser()
		{
			Console.Clear();
			Console.WriteLine();
			Console.ForegroundColor = ConsoleColor.Green;
			Console.WriteLine(@"________            .___               _________.__              .__          __                ");
			Console.WriteLine(@"\_____  \_______  __| _/___________   /   _____/|__| _____  __ __|  | _____ _/  |_  ___________ ");
			Console.WriteLine(@" /   |   \_  __ \/ __ |/ __ \_  __ \  \_____  \ |  |/     \|  |  \  | \__  \\   __\/  _ \_  __ \");
			Console.WriteLine(@"/    |    \  | \/ /_/ \  ___/|  | \/  /        \|  |  Y Y  \  |  /  |__/ __ \|  | (  <_> )  | \/");
			Console.WriteLine(@"\_______  /__|  \____ |\___  >__|    /_______  /|__|__|_|  /____/|____(____  /__|  \____/|__|   ");
			Console.WriteLine(@"        \/           \/    \/                \/          \/                \/                   ");
			Console.ForegroundColor = ConsoleColor.White;
			Console.WriteLine();
		}


	}

}