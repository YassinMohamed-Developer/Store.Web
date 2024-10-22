using AutoMapper;
using Microsoft.Extensions.Configuration;
using Store.Data.Entity;
using Store.Data.Entity.OrderEntity;
using Stripe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Product = Store.Data.Entity.Product;

namespace Store.Service.Services.ProductService.Dto
{
    public class ProductProfile : Profile
    {

        public ProductProfile()
        {
            //Make auto mapper between Product and ProductDetailsDto but use two method is {ForMember,MapFrom }
            //because ProductDetailsDto have {BrandName,TypeName} that not in Product   
            CreateMap<Product, ProductDetailsDto>()
                .ForMember(dest => dest.BrandName, option => option.MapFrom(src => src.Brand.Name))
                .ForMember(dest => dest.TypeName, option => option.MapFrom(src => src.Type.Name))
                //.ForMember(dest => dest.PictureUrl,option => option.MapFrom(src => $"{_configuration["BaseUrl"]}/{src.PictureUrl}"))
                .ForMember(dest => dest.PictureUrl, option => option.MapFrom<ProductPictureUrlResolver>());

            CreateMap<ProductBrand, BrandTypeDetailsDto>();

            CreateMap<ProductType, BrandTypeDetailsDto>();
        } 
    }
}
