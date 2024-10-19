using AutoMapper;
using Microsoft.Extensions.Configuration;
using Store.Data.Entity;
using Store.Service.Services.ProductService.Dto;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Service.Services.ProductService
{
    public class ProductPictureUrlResolver : IValueResolver<Product, ProductDetailsDto, string>
    {
        private readonly IConfiguration _configuration;

        public ProductPictureUrlResolver(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public string Resolve(Product source, ProductDetailsDto destination, string destMember, ResolutionContext context)
        {
            if (!string.IsNullOrEmpty(source.PictureUrl))
            {
                //Kda ana 3mlt concat le localhost bta3y be url bta3 image
                return $"{_configuration["BaseUrl"]}/{source.PictureUrl}";
            }
            return null;
        }
    }
}
