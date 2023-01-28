using AutoMapper;
using Frisbee.ApiModels;
using FrisbeeApp.DatabaseModels.Models;

namespace FrisbeeApp.Controllers.Mappers
{
    public class RegisterApiModelProfile : Profile
    {
        public RegisterApiModelProfile()
        {
            CreateMap<RegisterApiModel, User>();
        }

    }
}
