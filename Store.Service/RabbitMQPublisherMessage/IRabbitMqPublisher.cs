using Store.Service.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Service.RabbitMQPublisherMessage
{
	public interface IRabbitMqPublisher
	{
		Task PublishInvoiceAsync(OrderCreatedEvent orderCreatedEvent);

		Task PublishEmailAsync(EmailEvent emailEvent);
	}
}
