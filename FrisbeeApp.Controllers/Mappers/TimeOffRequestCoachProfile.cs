using AutoMapper;
using Frisbee.ApiModels;
using FrisbeeApp.DatabaseModels.Models;
using FrisbeeApp.Logic.DtoModels;

namespace FrisbeeApp.Controllers.Mappers
{
    public class TimeOffRequestCoachProfile : Profile
    {
        public TimeOffRequestCoachProfile()
        {
            CreateMap<TimeOffRequest, TimeOffRequestCoachDTO>();
        }
    }
}
