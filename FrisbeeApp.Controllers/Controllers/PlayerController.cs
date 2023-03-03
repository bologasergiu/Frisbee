﻿using AutoMapper;
using Frisbee.ApiModels;
using FrisbeeApp.DatabaseModels.Models;
using FrisbeeApp.EmailSender.Abstractions;
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
        private readonly IEmailService _emailServiceRepository;
        private List<string> coachesList;


        public PlayerController(IMapper mapper, IPlayerRepository playerRepository, IUserRepository userRepository, IEmailService emailServiceRepository)
        {
            _mapper = mapper;
            _playerRepository = playerRepository;
            _userRepository = userRepository;
            _emailServiceRepository = emailServiceRepository;
        }

        [Authorize(Roles="Player")]
        [Route("add-time-off-request")]
        [HttpPost]
        public async Task<bool> AddTimeOffRequest(TimeOffRequestApiModel timeOffRequestApiModel)
        {
            var timeOffRequest = _mapper.Map<TimeOffRequest>(timeOffRequestApiModel);
            var coachesList = _playerRepository.GetCochEmailList(User.Identity.Name);
            _emailServiceRepository.SendTimeOffRequestEmail(EmailTemplateType.TimeOffRequest, await coachesList, new EmailSender.Common.TimeOffRequestModel
            {
                StartDate = timeOffRequestApiModel.StartDate,
                EndDate = timeOffRequestApiModel.EndDate,
                Type = timeOffRequestApiModel.RequestType.ToString()
            });

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
        public async Task<bool> CancelledTimeOffRequest(Guid Id)
        {
            return await _playerRepository.CancelledTimeOffRequest(Id);
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
