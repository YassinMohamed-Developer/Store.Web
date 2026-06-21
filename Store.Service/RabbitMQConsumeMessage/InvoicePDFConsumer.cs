using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Store.Service.Helper;
using Store.Service.Helper.GeneratePdf;
using Store.Service.RabbitMQPublisherMessage;
using Store.Service.Services.OrderService;
using Store.Service.Services.OrderService.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Vax.Service.DTOS.RequestDto;
using Vax.Service.Interface;

namespace Store.Service.RabbitMQConsumeMessage
{
	public class InvoicePDFConsumer : BackgroundService
	{
		private readonly IConfiguration _configuration;
		private readonly IServiceScopeFactory _serviceScopeFactory;

		public InvoicePDFConsumer(IConfiguration configuration,
			IServiceScopeFactory serviceScopeFactory)
		{
			_configuration = configuration;
			_serviceScopeFactory = serviceScopeFactory;

		}

		protected override async Task ExecuteAsync(CancellationToken stoppingToken)
		{
			var factory = new ConnectionFactory
			{
				HostName = _configuration["RabbitMQ:Host"]
			};

			using var connection = await factory.CreateConnectionAsync();

			using var channel = await connection.CreateChannelAsync();

			await channel.QueueDeclareAsync(
				queue: "InvoiceQueue",
				durable: true,
				autoDelete: false,
				exclusive: false
			);

			var consumer = new AsyncEventingBasicConsumer(channel);

			consumer.ReceivedAsync += async (sender, arg) =>
			{
				var body = arg.Body.ToArray();
				
				var json = Encoding.UTF8.GetString(body);

				var message = JsonSerializer.Deserialize<OrderCreatedEvent>(json);

				using var scope = _serviceScopeFactory.CreateScope();

				var orderservice = scope.ServiceProvider.GetRequiredService<IOrderService>();

				var order = await orderservice.GetOrderByIdAsync(message.OrderId);


				var orderitems = order.OrderItems.Select(item => new InvoiceItemDto
				{
					ProductName = item.ProductName,
					Price = item.Price,
					Quantity = item.Quantity
				}).ToList();

				var pdfservice = scope.ServiceProvider.GetRequiredService<IInvoicePdfGenerator>();

				var pdf = pdfservice.GenerateInvoice(message.OrderId,order.BuyerEmail,order.SubTotal,orderitems);

				Directory.CreateDirectory("Invoices");

				var filePath =
					Path.Combine(
						"Invoices",
						$"Invoice-{order.Id}.pdf");

				await File.WriteAllBytesAsync(filePath,pdf);

				var publisher = scope.ServiceProvider.GetRequiredService<IRabbitMqPublisher>();

				await publisher.PublishEmailAsync(new EmailEvent
				{
					Email = order.BuyerEmail,
					OrderId = order.Id,
					InvoicePath = filePath
				});

				await channel.BasicAckAsync(arg.DeliveryTag, false);

			};


			await channel.BasicConsumeAsync(
				queue: "InvoiceQueue",
				autoAck: false,
				consumer: consumer
				);

			await Task.Delay(
			Timeout.Infinite,
			stoppingToken);
		}
	}
}
