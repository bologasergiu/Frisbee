using AutoMapper;
using FrisbeeApp.DatabaseModels.Models;
using FrisbeeApp.Logic.DtoModels;

namespace FrisbeeApp.Controllers.Mappers
{
    public class UserProfile : Profile
    {
        public UserProfile() 
        {
            CreateMap<User, TeamMemberDTO>();
        }
    }
}
