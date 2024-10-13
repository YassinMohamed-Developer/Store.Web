using Store.Data.Entity.OrderEntity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Service.Services.OrderService.Dto
{
    public class OrderDto
    {
        public string BuyerEmail { get; set; }

        [Required]
        public int DeliveryMethodId { get; set; }

        public string BasketId { get; set; }

        public AddressDto ShippingAddress { get; set; }

        public string? PaymentIntentId { get; set; }

    }
}
