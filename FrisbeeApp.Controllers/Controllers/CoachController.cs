using AutoMapper;
using Frisbee.ApiModels;
using FrisbeeApp.Context;
using FrisbeeApp.DatabaseModels.Models;
using FrisbeeApp.EmailSender.Abstractions;
using FrisbeeApp.Logic.Abstractions;
using FrisbeeApp.Logic.Common;
using FrisbeeApp.Logic.DtoModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FrisbeeApp.Controllers.Controllers
{
    [ApiController]
    [Authorize(Roles ="Coach")]
    [Route("api/[controller]")]
    public class CoachController : ControllerBase
    {
        private readonly FrisbeeAppContext _context;
        private readonly IMapper _mapper;
        private readonly ICoachRepository _coachRepository;
        private readonly IUserRepository _userRepository;
        private readonly IEmailService _emailService;
        private List<string> emailList;

        public CoachController(FrisbeeAppContext context, IMapper mapper, ICoachRepository coachRepository, IUserRepository userRepository, IEmailService emailService)
        {
            _context = context;
            _mapper = mapper;
            _coachRepository = coachRepository;
            _userRepository = userRepository;
            _emailService = emailService;
        }

        [Authorize(Roles ="Coach")]
        [Route("add-training")]
        [HttpPost]
        public async Task<bool> AddTraining(TrainingApiModel trainingApiModel)
        {
            var training = _mapper.Map<Training>(trainingApiModel);
            var teamList = await _coachRepository.GetTeamEmailList(User.Identity.Name);
            _emailService.SendEmail(EmailTemplateType.Training, teamList, new EmailSender.Common.EmailInfo
            {
                Date = training.Date.ToString(),
                Field = training.Field.ToString()
            }); 

            return await _coachRepository.AddTraining(training, User.Identity.Name);
        }

        [Authorize(Roles = "Coach")]
        [Route("view-all-timeoff-requests-per-team")]
        [HttpPost]
        public async Task<List<TimeOffRequest>> ViewAllTimeoffRequestPerTeam()
        {
            return await _coachRepository.ViewAllTimeOffRequestsPerTeam(User.Identity.Name);
        }

        [Authorize(Roles = "Coach")]
        [Route("change-timeoff-request-status")]
        [HttpPut]
        public async Task<bool> ChangeTimeoffRequestStatus(Guid Id, RequestStatus status)
        {
            var plyerEmail = await _coachRepository.GetTimeOffRequestEmailAddress(Id);
            _emailService.SendEmail(EmailTemplateType.TimeOffRequestChangeStatus, new List<string> { plyerEmail }, new EmailSender.Common.EmailInfo
            {
                RequestStatus = status.ToString(),
                RequestId = Id.ToString(),
            });
            return await _coachRepository.ChangeTimeoffRequestStatus(Id, status, User.Identity.Name);
        }
        
        [Authorize(Roles = "Coach")]
        [Route("view-coach-filtered-requests")]
        [HttpGet]
        public async Task<List<TimeOffRequestCoachDTO>> CoachFilteredRequests([FromQuery] SearchCriteriaApiModel searchCriteriaApiModel)
        {
            var searchCriteria = _mapper.Map<SearchCriteria>(searchCriteriaApiModel);

            var timeOffRequestsList = await _userRepository.ViewFilteredRequests(User.Identity.Name, searchCriteria);

            return _mapper.Map<List<TimeOffRequestCoachDTO>>(timeOffRequestsList);
        }
        
    }
}
