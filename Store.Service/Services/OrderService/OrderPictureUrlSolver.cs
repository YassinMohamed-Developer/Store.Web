using AutoMapper;
using AutoMapper.Execution;
using Castle.Core.Configuration;
using Microsoft.Extensions.Configuration;
using Store.Data.Entity.OrderEntity;
using Store.Service.Services.OrderService.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IConfiguration = Microsoft.Extensions.Configuration.IConfiguration;

namespace Store.Service.Services.OrderService
{
    public class OrderPictureUrlSolver : IValueResolver<OrderItem, OrderItemDto, string>
    {
        private readonly IConfiguration _configuration;

        public OrderPictureUrlSolver(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string Resolve(OrderItem source, OrderItemDto destination, string destMember, ResolutionContext context)
        {
            if (!string.IsNullOrEmpty(source.ProductItem.PictureUrl))
            {
                return $"{_configuration["BaseUrl"]}/{source.ProductItem.PictureUrl}";
            }
            return null;
        }
    }
}
