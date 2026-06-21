namespace Store.Service.RabbitMQPublisherMessage
{
	public class EmailEvent
	{
		public Guid OrderId { get; set; }

		public string? Email { get; set; }

		public string? InvoicePath { get; set; }
	}
}