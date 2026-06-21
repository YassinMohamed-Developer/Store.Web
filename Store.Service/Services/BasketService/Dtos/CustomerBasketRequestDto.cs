using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Service.Services.BasketService.Dtos
{
	public class CustomerBasketRequestDto
	{
		public int DeliveryMethodId { get; set; }

		public decimal ShippingPrice { get; set; }

		public ICollection<BasketItemDto> BasketItems { get; set; } = new List<BasketItemDto>();

		public string? PaymentIntentId { get; set; }

		public string? ClientSecret { get; set; }
	}
}
