﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Store.Repoistory.Specification.ProductSpecs;
using Store.Service.Services.ProductService;
using Store.Service.Services.ProductService.Dto;
using Store.Web.Helper;

namespace Store.Web.Controllers
{
    [Authorize]
    public class ProductsController : BasicsController
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
        [Cache(20)]
        public async Task<ActionResult<IReadOnlyList<ProductDetailsDto>>> GetAllProductsAsync([FromQuery]ProductSpecification specs)
            => Ok(await _productService.GetAllProductWithSpecificationAsync(specs));

        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<ProductDetailsDto>>> GetProductsById(int? Id)
            => Ok(await _productService.GetProductByIdAsync(Id));
    }
}
