using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Store.Service.Services.ProductService;
using Store.Service.Services.ProductService.Dto;

namespace Store.Web.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<BrandTypeDetailsDto>>> GetAllBrandsAsync()
            => Ok(await _productService.GetAllBrandsAsync());

        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<BrandTypeDetailsDto>>> GetAllTypesAsync()
            => Ok(await _productService.GetAllTypesAsync());

        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<ProductDetailsDto>>> GetAllProductsAsync()
            => Ok(await _productService.GetAllProductAsync());

        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<ProductDetailsDto>>> GetProductsById(int? id)
            => Ok(await _productService.GetProductByIdAsync(id.Value));
    }
}
