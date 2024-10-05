using Store.Repoistory.BasketRepository.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Service.Services.BasketService.Dtos
{
    public class CustomerBasketDto
    {
        public string? Id { get; set; }

        public int? DeliveryMethod { get; set; }

        public decimal ShippingPrice { get; set; }

        public ICollection<BasketItemDto> BasketItems { get; set; } = new List<BasketItemDto>();
    }
}
