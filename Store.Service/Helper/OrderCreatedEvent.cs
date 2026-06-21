using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Service.Helper
{
	public class OrderCreatedEvent
	{
		public Guid OrderId { get; set; }
		public string? CustomerEmail { get; set; }
	}
}
