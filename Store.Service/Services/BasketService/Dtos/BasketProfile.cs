using AutoMapper;
using Store.Repoistory.BasketRepository.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Service.Services.BasketService.Dtos
{
    public class BasketProfile : Profile
    {
        public BasketProfile()
        {
            CreateMap<CustomerBasket,CustomerBasketDto>().ReverseMap();
            CreateMap<BasketItem,BasketItemDto>().ReverseMap();
        }
    }
}
