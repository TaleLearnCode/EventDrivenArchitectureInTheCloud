namespace TaleLearnCode.EventDrivenArchitectureInTheCloud
{

	public class Order
	{

		public string Id { get; set; }

		public Customer Customer { get; set; }

		public PostalAddress ShippingAddress { get; set; }

		public string OrderItem { get; set; }

		public decimal OrderTotal { get; set; }

	}

}