using AutoMapper;
using FrisbeeApp.DatabaseModels.Models;
using FrisbeeApp.Logic.DtoModels;

namespace FrisbeeApp.Controllers.Mappers
{
    public class TeamModelProfile : Profile
    {
        public TeamModelProfile()
        {
            CreateMap<Team, TeamMemberDTO>();
        }
    }
}
