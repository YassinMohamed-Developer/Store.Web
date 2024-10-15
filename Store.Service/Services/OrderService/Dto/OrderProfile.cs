using AutoMapper;
using Store.Data.Entity.OrderEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Service.Services.OrderService.Dto
{
    public class OrderProfile : Profile
    {
        public OrderProfile()
        {
            CreateMap<OrderItem,OrderItemDto>()
                .ForMember(dest => dest.ProductId,option => option.MapFrom(x => x.ProductItem.ProductId))
                .ForMember(dest => dest.ProductName,option => option.MapFrom(x => x.ProductItem.ProductName))
                .ForMember(dest => dest.ProductName,option => option.MapFrom(x => x.ProductItem.PictureUrl))
                .ForMember(dest => dest.PictureUrl,option => option.MapFrom<OrderPictureUrlSolver>()).ReverseMap();

            CreateMap<ShippingAddress, AddressDto>().ReverseMap();

            CreateMap<Order, OrderDetailsDto>()
                .ForMember(dest => dest.DeliveryMethodName, option => option.MapFrom(x => x.DeliveryMethod.ShortName))
                .ForMember(dest => dest.ShippingPrice, option => option.MapFrom(x => x.DeliveryMethod.Price)).ReverseMap();
        }
    }
}
