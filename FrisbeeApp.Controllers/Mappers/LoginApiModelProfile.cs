using AutoMapper;
using Frisbee.ApiModels;
using FrisbeeApp.Logic.DtoModels;

namespace FrisbeeApp.Controllers.Mappers
{
    public class LoginApiModelProfile : Profile
    {
        public LoginApiModelProfile()
        {
            CreateMap<LoginApiModel, LoginDTO>();
        }
    }
}
