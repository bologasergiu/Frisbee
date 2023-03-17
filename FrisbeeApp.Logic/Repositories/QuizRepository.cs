using AutoMapper;
using Frisbee.ApiModels;
using FrisbeeApp.Context;
using FrisbeeApp.DatabaseModels.Models;
using FrisbeeApp.Logic.Abstractions;
using FrisbeeApp.Logic.Abstractisations;
using FrisbeeApp.Logic.DtoModels;
using Microsoft.EntityFrameworkCore;

namespace FrisbeeApp.Logic.Repositories
{
    public class QuizRepository : IQuizRepository
    {
        private readonly FrisbeeAppContext _context;
        private readonly IMapper _mapper;

        public QuizRepository(FrisbeeAppContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<bool> AddQuestion(QuestionApiModel questionApiModel) 
        {  
            var question = _mapper.Map<Question>(questionApiModel);
            question.Id = Guid.NewGuid();
            await _context.QuizQuestions.AddAsync(question);
            var result = await _context.SaveChangesAsync();
            if (result > 0)
                return true;

            return false;
        }
        public async Task<bool> DeleteQuestion(Guid Id)
        {
            var question = await _context.QuizQuestions.FirstOrDefaultAsync(x=> x.Id == Id);
            if(question != null)
            {
                _context.QuizQuestions.Remove(question);
                var result = await _context.SaveChangesAsync();
                return result > 0;
            }
            return false;
        }

        public async Task<List<QuestionDTO>> GetQuestions()
        {
            var questionList = await _context.QuizQuestions.ToListAsync();
            List<QuestionDTO> questionListDTO = new List<QuestionDTO>();
            foreach (var question in questionList)
            {
                var dto = _mapper.Map<QuestionDTO>(question);
                questionListDTO.Add(dto);
            }
            return questionListDTO;
        }

        public async Task<List<QuestionDTO>> GetQuizQuestions()
        {
            var questionList = await _context.QuizQuestions.OrderBy(x => Guid.NewGuid()).Take(10).ToListAsync();

            List<QuestionDTO> questionListDTO = new List<QuestionDTO>();
            foreach (var question in questionList)
            {
                var dto = _mapper.Map<QuestionDTO>(question);
                questionListDTO.Add(dto);
            }
            return questionListDTO;
        }
    }
}
