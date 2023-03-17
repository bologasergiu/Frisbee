using AutoMapper;
using Frisbee.ApiModels;
using FrisbeeApp.DatabaseModels.Models;

namespace FrisbeeApp.Controllers.Mappers
{
    public class QuestionApiModelProfile : Profile
    {
        public QuestionApiModelProfile()
        {
            CreateMap<QuestionApiModel, Question>();
        }
    }
}
