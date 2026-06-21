using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Store.Service.RabbitMQPublisherMessage;
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
	public class EmailConsumer : BackgroundService
	{
		private readonly IConfiguration _configuration;
		private readonly IServiceScopeFactory _serviceScopeFactory;

		public EmailConsumer(IConfiguration configuration, IServiceScopeFactory serviceScopeFactory)
		{
			_configuration = configuration;
			_serviceScopeFactory = serviceScopeFactory;
		}
		protected async override Task ExecuteAsync(CancellationToken stoppingToken)
		{
			var factory = new ConnectionFactory
			{
				HostName = _configuration["RabbitMQ:Host"]
			};

			using var connection = await factory.CreateConnectionAsync();

			using var channel = await connection.CreateChannelAsync();

			await channel.QueueDeclareAsync(
				queue: "emailQueue",
				durable: true,
				autoDelete: false,
				exclusive: false
			);

			var consumer = new AsyncEventingBasicConsumer(channel);

			consumer.ReceivedAsync += async (sender, arg) =>
			{
				var body = arg.Body.ToArray();

				var json = Encoding.UTF8.GetString(body);

				var message = JsonSerializer.Deserialize<EmailEvent>(json);

				using var scope = _serviceScopeFactory.CreateScope();

				var emailService = scope.ServiceProvider.GetRequiredService<IEmailService>();

				emailService.SendEmail(new EmailDto
				{
					To =  message.Email,
					Subject = "Order Confirmation",
					Body = $"Your order with ID {message.OrderId} has been confirmed.",
					AttachmentPath = message.InvoicePath
				});
			};

			await channel.BasicConsumeAsync(
				queue: "emailQueue",
				autoAck: true,
				consumer: consumer
			);

			await Task.Delay(Timeout.Infinite, stoppingToken);
		}
}
}
