using AutoMapper;
using Frisbee.ApiModels;
using FrisbeeApp.DatabaseModels.Models;

namespace FrisbeeApp.Controllers.Mappers
{
    public class RegisterApiModelProfile : Profile
    {
        public RegisterApiModelProfile()
        {
            CreateMap<RegisterApiModel, User>()
               .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.Email));
        }

    }
}
