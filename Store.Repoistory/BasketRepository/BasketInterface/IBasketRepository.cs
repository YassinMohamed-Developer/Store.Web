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

        Task<string> AddToBasket(CustomerBasket basket);
        Task<CustomerBasket> GetBasketAsync(string Id);

        Task<string> UpdateBasketAsync(CustomerBasket basket);


        Task<bool> DeleteBasketAsync(string Id);
    }
}
