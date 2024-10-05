using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Repoistory.BasketRepository.Models
{
    public class BasketItem
    {
        public int ProductId { get; set; }

        public string ProductName { get; set; }

        public decimal Price { get; set; }

        public int Quantity { get; set; }

        public string PictureUrl { get; set; }

        public string BrandName { get; set; }

        public string TypeName { get; set; }

        public CustomerBasket CustomerBasket { get; set; }

        public int? CustomerBasketId { get; set; }
    }
}
