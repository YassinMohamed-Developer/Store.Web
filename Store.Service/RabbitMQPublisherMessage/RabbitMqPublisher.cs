using Microsoft.Extensions.Configuration;
using RabbitMQ.Client;
using Store.Service.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Service.RabbitMQPublisherMessage
{
	public class RabbitMqPublisher : IRabbitMqPublisher
	{
		private readonly IConfiguration _configuration;

		public RabbitMqPublisher(IConfiguration configuration)
		{
			_configuration = configuration;
		}

		public async Task PublishEmailAsync(EmailEvent emailEvent)
		{
			var factory = new ConnectionFactory()
			{
				HostName = _configuration["RabbitMQ:Host"],
			};

			using var connection = await factory.CreateConnectionAsync();

			using var channel = await connection.CreateChannelAsync();

			await channel.QueueDeclareAsync(
				queue: "emailQueue",
				durable: true,
				autoDelete: false,
				exclusive: false
			);

			await channel.ExchangeDeclareAsync(

				exchange: "emailExchange",
				type: ExchangeType.Direct,
				durable: true,
				autoDelete: false
			);

			await channel.QueueBindAsync(
				queue: "emailQueue",
				exchange: "emailExchange",
				routingKey: "email_RoutingKey"
			);

			var body = Encoding.UTF8.GetBytes(System.Text.Json.JsonSerializer.Serialize(emailEvent));

			await channel.BasicPublishAsync(
				exchange: "emailExchange",
				routingKey: "email_RoutingKey",
				body: body
			);
		}

		public async Task PublishInvoiceAsync(OrderCreatedEvent orderCreatedEvent)
		{
			var factory = new ConnectionFactory()
			{
				HostName = _configuration["RabbitMQ:Host"],
			};

			using var connection = await factory.CreateConnectionAsync();

			using var channel = await connection.CreateChannelAsync();

			await channel.QueueDeclareAsync(
				queue: "InvoiceQueue",
				durable: true,
				autoDelete: false,
				exclusive: false
			);

			await channel.ExchangeDeclareAsync(
				
				exchange: "InvoiceExchange",
				type: ExchangeType.Direct,
				durable: true,
				autoDelete: false
			);

			await channel.QueueBindAsync(
				queue: "InvoiceQueue",
				exchange: "InvoiceExchange",
				routingKey: "Invoice_RoutingKey"
			);

			var body = Encoding.UTF8.GetBytes(System.Text.Json.JsonSerializer.Serialize(orderCreatedEvent));

			await channel.BasicPublishAsync(
				exchange: "InvoiceExchange",
				routingKey: "Invoice_RoutingKey",
				body: body
			);
		}
		
	}
}
