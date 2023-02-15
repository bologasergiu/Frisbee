using AutoMapper;
using Frisbee.ApiModels;
using FrisbeeApp.DatabaseModels.Models;
using FrisbeeApp.Logic.Abstractions;
using FrisbeeApp.Logic.Common;
using FrisbeeApp.Logic.DtoModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace FrisbeeApp.Controllers.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PlayerController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IPlayerRepository _playerRepository;
        private readonly IUserRepository _userRepository;


        public PlayerController(IMapper mapper, IPlayerRepository playerRepository, IUserRepository userRepository)
        {
            _mapper = mapper;
            _playerRepository = playerRepository;
            _userRepository = userRepository;
        }

        [Authorize(Roles="Player")]
        [Route("add-time-off-request")]
        [HttpPost]
        public async Task<bool> AddTimeOffRequest(TimeOffRequestApiModel timeOffRequestApiModel)
        {
            var timeOffRequest = _mapper.Map<TimeOffRequest>(timeOffRequestApiModel);

            return await _playerRepository.AddTimeOffRequest(timeOffRequest, User.Identity.Name);
        }

        [Authorize(Roles = "Player, Coach")]
        [Route("view-all-timeoff-requests")]
        [HttpGet]
        public async Task <List<TimeOffRequest>> ViewAllTimeoffRequests()
        {
            return await _playerRepository.ViewAllTimeOffRequest(User.Identity.Name);
        }

        [Authorize(Roles = "Player")]
        [Route("delete-timeoff-request")]
        [HttpPut]
        public async Task<bool> DeleteTimeOffRequest(Guid Id)
        {
            return await _playerRepository.DeleteTimeOffRequest(Id);
        }

        [Authorize(Roles = "Player")]
        [Route("view-player-filtered-requests")]
        [HttpGet]
        public async Task<List<TimeOffRequestPlayerDTO>> PlayerFilteredRequests([FromQuery] SearchCriteriaApiModel searchCriteriaApiModel)
        {
            var searchCriteria = _mapper.Map<SearchCriteria>(searchCriteriaApiModel);

            var timeOffRequestsList = await _userRepository.ViewFilteredRequests(User.Identity.Name, searchCriteria);

            return _mapper.Map<List<TimeOffRequestPlayerDTO>>(timeOffRequestsList);
        }
    }
}
