using AutoMapper;
using Frisbee.ApiModels;
using FrisbeeApp.DatabaseModels.Models;

namespace FrisbeeApp.Controllers.Mappers
{
    public class TrainingProfile : Profile
    {
        public TrainingProfile()
        {
            CreateMap<TrainingApiModel, Training>();
        }
    }
}
