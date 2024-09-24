using AutoMapper;
using Store.Data.Entity;
using Store.Repoistory.Interfaces;
using Store.Service.Services.ProductService.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Service.Services.ProductService
{
    public class ProductService : IProductService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ProductService(IUnitOfWork unitOfWork,IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<IReadOnlyList<BrandTypeDetailsDto>> GetAllBrandsAsync()
        {
            var brands = await _unitOfWork.Repository<ProductBrand,int>().GetAllAsNoTrackingAsync();

            //IReadOnlyList<BrandTypeDetailsDto> mappedbrands = brands.Select(x => new BrandTypeDetailsDto
            //{
            //    Id = x.Id,
            //    Name = x.Name,
            //    CreateAt = x.CreateAt,
            //}).ToList();

            var mappedbrands = _mapper.Map<IReadOnlyList<BrandTypeDetailsDto>>(brands);

            return mappedbrands;
        }

        public async Task<IReadOnlyList<ProductDetailsDto>> GetAllProductAsync()
        {
            var products = await _unitOfWork.Repository<Product, int>().GetAllAsNoTrackingAsync();

            //IReadOnlyList<ProductDetailsDto> mappedproduct = products.Select(x => new ProductDetailsDto
            //{
            //    Id = x.Id,
            //    Name = x.Name,
            //    CreateAt = x.CreateAt,
            //    BrandName = x.Brand.Name,
            //    TypeName = x.Type.Name,
            //    PictureUrl = x.PictureUrl,
            //    Description = x.Description,
            //    Price = x.Price,
            //}).ToList();

            var mappedproduct = _mapper.Map<IReadOnlyList<ProductDetailsDto>>(products);

            return mappedproduct;
        }

        public async Task<IReadOnlyList<BrandTypeDetailsDto>> GetAllTypesAsync()
        {
            var types = await _unitOfWork.Repository<ProductType, int>().GetAllAsNoTrackingAsync();

            //IReadOnlyList<BrandTypeDetailsDto> mappedType = types.Select(x => new BrandTypeDetailsDto
            //{
            //    Id = x.Id,
            //    Name = x.Name,
            //    CreateAt = x.CreateAt
            //}).ToList();

            var mappedType = _mapper.Map< IReadOnlyList<BrandTypeDetailsDto>>(types);

            return mappedType;
        }

        public async Task<ProductDetailsDto> GetProductByIdAsync(int? productid)
        {

            if(productid is null)
            {
                throw new Exception("Id is null");
            }
            var product = await _unitOfWork.Repository<Product, int>().GetByIdAsync(productid.Value);

            //var mappedproduct = new ProductDetailsDto
            //{
            //    Id = product.Id,
            //    Name = product.Name,
            //    CreateAt = product.CreateAt,
            //    BrandName = product.Brand.Name,
            //    TypeName = product.Type.Name,
            //    PictureUrl = product.PictureUrl,
            //    Description = product.Description,
            //    Price = product.Price,
            //};

            var mappedproduct = _mapper.Map<ProductDetailsDto>(product);

            return mappedproduct;
        }
    }
}
