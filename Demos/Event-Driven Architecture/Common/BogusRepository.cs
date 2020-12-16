using Bogus;
using System;

namespace TaleLearnCode.EventDrivenArchitectureInTheCloud
{

	public static class BogusRepository
	{

		public static Order GetOrder()
		{
			Randomizer.Seed = new Random();

			var orderGenerator = new Faker<Order>()
				.RuleFor(o => o.Id, Guid.NewGuid().ToString())
				.RuleFor(o => o.Customer, GetCustomer())
				.RuleFor(o => o.ShippingAddress, GetShippingAddress())
				.RuleFor(o => o.OrderItem, f => f.Commerce.Product())
				.RuleFor(o => o.OrderTotal, f => f.Commerce.Price(1, 100));

			return orderGenerator.Generate(1)[0];

		}

		private static Customer GetCustomer()
		{
			Randomizer.Seed = new Random();

			var customerGenerator = new Faker<Customer>()
				.RuleFor(c => c.Id, Guid.NewGuid().ToString())
				.RuleFor(c => c.FirstName, f => f.Person.FirstName)
				.RuleFor(c => c.LastName, f => f.Person.LastName)
				.RuleFor(c => c.EmailAddress, f => f.Internet.Email());

			return customerGenerator.Generate(1)[0];
		}

		private static PostalAddress GetShippingAddress()
		{

			Randomizer.Seed = new Random();

			var shippingAddressGenerator = new Faker<PostalAddress>()
				.RuleFor(a => a.StreetAddress, f => f.Address.StreetAddress())
				.RuleFor(a => a.City, f => f.Address.City())
				.RuleFor(a => a.State, f => f.Address.StateAbbr())
				.RuleFor(a => a.PostalCode, f => f.Address.ZipCode());

			return shippingAddressGenerator.Generate(1)[0];

		}


	}

}