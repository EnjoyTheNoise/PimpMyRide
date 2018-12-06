using AutoMapper;
using PimpMyRide.Core.Api.Tokens.Dto;
using PimpMyRide.Core.Cars.Dto;
using PimpMyRide.Core.Data.Models;
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

            CreateMap<UserRegisterRequestDto, User>();

            CreateMap<TokenDto, RefreshTokenRequestDto>();
            CreateMap<TokenDto, RevokeTokenRequestDto>();
            CreateMap<TokenCreateDto, UserLoginRequestDto>();
        }
    }
}
