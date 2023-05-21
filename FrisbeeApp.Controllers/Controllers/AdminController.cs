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
        private readonly IQuizRepository _quizRepository;

        public AdminController(IAdminRepository adminRepository, IQuizRepository quizRepository)
        {
            _adminRepository = adminRepository;
            _quizRepository = quizRepository;
        }

        [HttpPost]
        [Route("add-team/{teamName}")]
        public async Task<bool> AddTeam(string teamName)
        {
            return await _adminRepository.CreateTeam(teamName);
        }

        [HttpPut]
        [Route("delete-user/{email}")]
        public async Task<bool> DeleteUser(string email)
        {
            return await _adminRepository.DeleteUser(email);
        }

        [HttpPost]
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

        [HttpPut]
        [Route("delete-team/{name}")]
        public async Task<bool> DeleteTeam(string name)
        {
            return await _adminRepository.DeleteTeam(name);
        }

        [HttpGet]
        [Route("get-all-teams")]
        public async Task<List<TeamModelDTO>> GetAllTeams()
        {
            List<TeamModelDTO> teams = new List<TeamModelDTO>();
            teams = await _adminRepository.GetAllTeams();
            return teams;
        }

        [HttpGet]
        [Route("get-all-users")]
        public async Task<List<TeamMemberDTO>> GetAllUsers()
        {
            List<TeamMemberDTO> users = new List<TeamMemberDTO>();
            users = await _adminRepository.GetAllUsers();
            return users;
        }

        [HttpGet]
        [Route("get-team")]
        public async Task<TeamModelDTO> GetTeam(string teamName)
        {
            return await _adminRepository.GetTeamModel(teamName);
        }

    }
   
}
