using AutoMapper;
using Frisbee.ApiModels;
using FrisbeeApp.DatabaseModels.Models;
using FrisbeeApp.Logic.Abstractions;
using FrisbeeApp.Logic.DtoModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FrisbeeApp.Controllers.Controllers
{
    [ApiController]
    [Authorize(Roles = "Admin")]
    [Route("api/[controller]")]
    public class AdminController : ControllerBase
    {
        private readonly IAdminRepository _adminRepository;
        private readonly IMapper _mapper;
        private readonly IQuizRepository _quizRepository;

        public AdminController(IAdminRepository adminRepository, IMapper mapper, IQuizRepository quizRepository)
        {
            _adminRepository = adminRepository;
            _mapper = mapper;
            _quizRepository = quizRepository;
        }

        [HttpPost]
        [Route("create-team")]
        public async Task<bool> CreateTeam(string teamName)
        {
            return await _adminRepository.CreateTeam(teamName);
        }

        [HttpPut]
        [Route("delete-user")]
        public async Task<bool> DeleteUser(Guid Id)
        {
            return await _adminRepository.DeleteUser(Id);
        }

        [HttpPut]
        [Route("add-question")]
        public async Task<bool> AddQuestion(QuestionApiModel questionApiModel)
        {
           
            return await _quizRepository.AddQuestion(questionApiModel);
        }

        [HttpPut]
        [Route("delete-question")]
        public async Task<bool> DeleteQuestion(Guid Id)
        {

            return await _quizRepository.DeleteQuestion(Id);
        }

        [HttpGet]
        [Route("get-questions")]
        public async Task<List<QuestionDTO>> GetQuestions()
        {

            return await _quizRepository.GetQuestions();
        }

    }
}
