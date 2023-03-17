using Frisbee.ApiModels;
using FrisbeeApp.DatabaseModels.Models;
using FrisbeeApp.Logic.DtoModels;

namespace FrisbeeApp.Logic.Abstractions
{
    public interface IQuizRepository
    {
        Task<bool> AddQuestion(QuestionApiModel questionApiModel);
        Task<bool> DeleteQuestion(Guid Id);
        Task<List<QuestionDTO>> GetQuestions();
        Task<List<QuestionDTO>> GetQuizQuestions();
    }
}
