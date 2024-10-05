using Store.Repoistory.BasketRepository.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Repoistory.BasketRepository.BasketInterface
{
    public interface IBasketRepository
    {
        Task<CustomerBasket> GetBasketAsync(string Id);

        Task<CustomerBasket> UpdateBasketAsync(CustomerBasket basket);


        Task<bool> DeleteBasketAsync(string Id);
    }
}
