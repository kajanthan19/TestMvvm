using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestMvvm.Domain.Dtos;
using TestMvvm.Domain.Entities;

namespace TestMvvm.Core.Mapper
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            this.CreateMap<CreateAircraftDto, Aircraft>().ReverseMap();
            this.CreateMap<Aircraft, AircraftDto>().ReverseMap();

            //this.CreateMap<CreatePersonDto, Person>().ReverseMap();
            //this.CreateMap<Person, PersonDto>().ReverseMap();

            //this.CreateMap<CreateOrderDto, Order>().ReverseMap();
            //this.CreateMap<Order, OrderDto>().ReverseMap();
        }
    }
}
