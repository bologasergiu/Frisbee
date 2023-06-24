using AutoMapper;
using FrisbeeApp.DatabaseModels.Models;
using FrisbeeApp.Logic.DtoModels;

namespace FrisbeeApp.Controllers.Mappers
{
    public class TrainingDtoProfile : Profile
    {
        public TrainingDtoProfile()
        {
            CreateMap<Training, TrainingDTO>();
        }
    }
}
