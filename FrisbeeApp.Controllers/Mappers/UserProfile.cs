using AutoMapper;
using Frisbee.ApiModels;
using FrisbeeApp.DatabaseModels.Models;
using FrisbeeApp.Logic.DtoModels;

namespace FrisbeeApp.Controllers.Mappers
{
    public class UserProfile : Profile
    {
        public UserProfile() 
        {
            CreateMap<User, TeamMemberDTO>();
            CreateMap<UpdateUserApiModel, User>().ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.Email));           
        }
    }
}
