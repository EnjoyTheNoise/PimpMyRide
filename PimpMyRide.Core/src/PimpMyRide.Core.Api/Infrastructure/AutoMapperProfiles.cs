using AutoMapper;
using PimpMyRide.Core.Api.Tokens.Dto;
using PimpMyRide.Core.Cars.Dto;
using PimpMyRide.Core.Data.Models;
using PimpMyRide.Core.RentCars.Dto;
using PimpMyRide.Core.Tokens.Dto;
using PimpMyRide.Core.Users.Dto;


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

            CreateMap<RentCar, RentCarResponseDto>();
            CreateMap<RentCarRequestDto, RentCar>();

            CreateMap<RentCar, CancelRentCarResponseDto>();
            CreateMap<CancelRentCarRequestDto, RentCar>();

            CreateMap<UserRegisterRequestDto, User>();

            CreateMap<TokenDto, RefreshTokenRequestDto>();
            CreateMap<TokenDto, RevokeTokenRequestDto>();
            CreateMap<TokenCreateDto, UserLoginRequestDto>();
        }
    }
}
