using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Store.Service.Services.BasketService;
using Store.Service.Services.BasketService.Dtos;

namespace Store.Web.Controllers
{
    public class BasketController : BasicsController
    {
        private readonly IBasketService _basketService;

        public BasketController(IBasketService basketService)
        {
            _basketService = basketService;
        }

        [HttpGet("{Id}")]
        public async Task<ActionResult<CustomerBasketDto>> GetBasketAsync(string Id)
            => Ok(await _basketService.GetBasketAsync(Id));

        [HttpPost]
        public async Task<ActionResult<CustomerBasketDto>> UpdateBasketAsync(CustomerBasketDto basketDto)
            => Ok(await _basketService.UpdateBasketAsync(basketDto));

        [HttpDelete]
        public async Task<ActionResult<bool>> DeleteBasketAsync(string Id)
            => Ok(await _basketService.DeleteBasketAsync(Id));
    }
}
    