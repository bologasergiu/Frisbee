using AutoMapper;
using Frisbee.ApiModels;
using FrisbeeApp.DatabaseModels.Models;
using FrisbeeApp.Logic.Abstractions;
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
    }
}
