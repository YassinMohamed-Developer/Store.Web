using Store.Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Repoistory.BasketRepository.Models
{
    public class CustomerBasket
    {
        public string Id { get; set; }

        public int? DeliveryMethod { get; set; }

        public decimal ShippingPrice { get; set; }

        public ICollection<BasketItem> BasketItems { get; set; } = new List<BasketItem>();

        public string? PaymentIntentId {  get; set; }

        public string? ClientSecret { get; set; }
    }
}
