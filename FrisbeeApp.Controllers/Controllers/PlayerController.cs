﻿using AutoMapper;
using Frisbee.ApiModels;
using FrisbeeApp.DatabaseModels.Models;
using FrisbeeApp.EmailSender.Abstractions;
using FrisbeeApp.Logic.Abstractions;
using FrisbeeApp.Logic.Common;
using FrisbeeApp.Logic.DtoModels;
using FrisbeeApp.Logic.Repositories;
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
        private readonly IQuizRepository _quizRepository;


        public PlayerController(IMapper mapper, IPlayerRepository playerRepository, IUserRepository userRepository, IEmailService emailServiceRepository, IQuizRepository quizRepository)
        {
            _mapper = mapper;
            _playerRepository = playerRepository;
            _userRepository = userRepository;
            _emailServiceRepository = emailServiceRepository;
            _quizRepository = quizRepository;
        }

        [Authorize(Roles="Player")]
        [Route("add-time-off-request")]
        [HttpPost]
        public async Task<bool> AddTimeOffRequest(TimeOffRequestApiModel timeOffRequestApiModel)
        {
            var timeOffRequest = _mapper.Map<TimeOffRequest>(timeOffRequestApiModel);
            var coachesList = await _playerRepository.GetCoachEmail(User.Identity.Name);
            _emailServiceRepository.SendEmail(EmailTemplateType.TimeOffRequest, new List<string> { coachesList }, new EmailSender.Common.EmailInfo
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
        [Route("delete-timeoff-request/{id}")]
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

        [Authorize(Roles = "Player")]
        [Route("get-quiz-questions")]
        [HttpGet]
        public async Task<List<QuestionDTO>> GetQuizQuestions()
        {

            return await _quizRepository.GetQuizQuestions();
        }

        [Authorize(Roles = "Player")]
        [Route("get-trainings")]
        [HttpGet]
        public async Task<List<TrainingDTO>> GetTrainings()
        {
            var trainigs = await _playerRepository.GetTrainings(User.Identity.Name);
            var taringDtoList = new List<TrainingDTO>();

            foreach (var training in trainigs) { 
                var trainingDto = _mapper.Map<TrainingDTO>(training);
                taringDtoList.Add(trainingDto);
            }
            return taringDtoList;

        }
    }
}
