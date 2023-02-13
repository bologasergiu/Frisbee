using AutoMapper;
using Frisbee.ApiModels;
using FrisbeeApp.DatabaseModels.Models;
using FrisbeeApp.Logic.Abstractions;
using FrisbeeApp.Logic.Exceptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Runtime.CompilerServices;

namespace FrisbeeApp.Controllers.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PlayerController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IPlayerRepository _repository;

        public PlayerController(IMapper mapper, IPlayerRepository repository)
        {
            _mapper = mapper;
            _repository = repository;
        }

        [Authorize(Roles="Player")]
        [Route("add-time-off-request")]
        [HttpPost]
        public async Task<bool> AddTimeOffRequest(TimeOffRequestApiModel timeOffRequestApiModel)
        {
            var timeOffRequest = _mapper.Map<TimeOffRequest>(timeOffRequestApiModel);

            return await _repository.AddTimeOffRequest(timeOffRequest, User.Identity.Name);
        }

        [Authorize(Roles = "Player, Coach")]
        [Route("view-all-timeoff-requests")]
        [HttpGet]
        public async Task <List<TimeOffRequest>> ViewAllTimeoffRequests()
        {
            return await _repository.ViewAllTimeOffRequest(User.Identity.Name);
        }

        [Authorize(Roles = "Player")]
        [Route("delete-timeoff-request")]
        [HttpPut]
        public async Task<bool> DeleteTimeOffRequest(Guid Id)
        {
            return await _repository.DeleteTimeOffRequest(Id);
        }
    }
}
