using AutoMapper;
using FrisbeeApp.DatabaseModels.Models;
using FrisbeeApp.Logic.DtoModels;

namespace FrisbeeApp.Controllers.Mappers
{
    public class QuizQuestionProfile : Profile
    {
        public QuizQuestionProfile()
        {
            CreateMap<Question, QuestionDTO>();
        }
    }
}
