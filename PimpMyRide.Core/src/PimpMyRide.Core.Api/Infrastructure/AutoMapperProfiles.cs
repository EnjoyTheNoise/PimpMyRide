using AutoMapper;
using PimpMyRide.Core.Cars.Dto;
using PimpMyRide.Core.Data.Models;


namespace PimpMyRide.Core.Api.Infrastructure
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<Car, AddCarResponseDto>();
            CreateMap<AddCarRequestDto, Car>();

            CreateMap<Car, GetCarByIdResponse>();
            CreateMap<GetCarByIdRequest, Car>();
        }
    }
}
