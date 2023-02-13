using AutoMapper;
using Frisbee.ApiModels;
using FrisbeeApp.DatabaseModels.Models;
using FrisbeeApp.Logic.Abstractions;
using FrisbeeApp.Logic.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FrisbeeApp.Controllers.Controllers
{
    [ApiController]
    [Authorize(Roles ="Coach")]
    [Route("api/[controller]")]
    public class CoachController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly ICoachRepository _repository;

        public CoachController(IMapper mapper, ICoachRepository repository)
        {
            _mapper = mapper;
            _repository = repository;
        }

        [Authorize(Roles ="Coach")]
        [Route("add-training")]
        [HttpPost]
        public async Task<bool> AddTraining(TrainingApiModel trainingApiModel)
        {
            var training = _mapper.Map<Training>(trainingApiModel);

            return await _repository.AddTraining(training, User.Identity.Name);
        }

        [Authorize(Roles = "Coach")]
        [Route("view-all-timeoff-requests-per-team")]
        [HttpPost]
        public async Task<List<TimeOffRequest>> ViewAllTimeoffRequestPerTeam()
        {
            return await _repository.ViewAllTimeoffRequestPerTeam(User.Identity.Name);
        }

        [Authorize(Roles = "Coach")]
        [Route("change-timeoff-request-status")]
        [HttpPut]
        public async Task<bool> ChangeTimeoffRequestStatus(Guid Id, RequestStatus status)
        {
            return await _repository.ChangeTimeoffRequestStatus(Id, status);
        }
    }
}
