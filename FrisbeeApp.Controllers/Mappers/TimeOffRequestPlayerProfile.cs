using AutoMapper;
using FrisbeeApp.DatabaseModels.Models;
using FrisbeeApp.Logic.DtoModels;

namespace FrisbeeApp.Controllers.Mappers
{
    public class TimeOffRequestPlayerProfile : Profile
    {
        public TimeOffRequestPlayerProfile() 
        {
            CreateMap<TimeOffRequest, TimeOffRequestPlayerDTO>();
        }
    }
}
