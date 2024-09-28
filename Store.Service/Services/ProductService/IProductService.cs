using Store.Repoistory.Specification.ProductSpecs;
using Store.Service.Helper;
using Store.Service.Services.ProductService.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Service.Services.ProductService
{
    public interface IProductService
    {
       Task<ProductDetailsDto> GetProductByIdAsync(int? productId);
        Task<ProductPagnatedDto<ProductDetailsDto>> GetAllProductWithSpecificationAsync(ProductSpecification specs);

        Task<IReadOnlyList<ProductDetailsDto>> GetAllProductAsync();

        Task<IReadOnlyList<BrandTypeDetailsDto>> GetAllBrandsAsync();

        Task<IReadOnlyList<BrandTypeDetailsDto>> GetAllTypesAsync();

    }
}
