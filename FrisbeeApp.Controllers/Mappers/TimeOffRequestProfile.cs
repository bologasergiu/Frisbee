using AutoMapper;
using Frisbee.ApiModels;
using FrisbeeApp.DatabaseModels.Models;

namespace FrisbeeApp.Controllers.Mappers
{
    public class TimeOffRequestProfile : Profile
    {
        public TimeOffRequestProfile()
        {
            CreateMap<TimeOffRequestApiModel, TimeOffRequest>();
        }
        
    }
}
