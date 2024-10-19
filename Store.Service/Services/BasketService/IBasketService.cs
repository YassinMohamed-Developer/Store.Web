using Store.Service.Services.BasketService.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Service.Services.BasketService
{
    public interface IBasketService
    {
        Task<CustomerBasketDto> GetBasketAsync(string Id);

        Task<CustomerBasketDto> UpdateBasketAsync(CustomerBasketDto basket);

        Task<bool> DeleteBasketAsync(string Id);
    }
}
