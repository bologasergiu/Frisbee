using AutoMapper;
using FrisbeeApp.Logic.Abstractions;
using FrisbeeApp.Logic.Abstractisations;
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

        [Route("approve-account")]
        [HttpPut]
        public async Task<bool> ApproveAccount(Guid id)
        {
            return await _repository.ApproveAccount(id);
        }

    }
}
