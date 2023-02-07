using AutoMapper;
using FrisbeeApp.Logic.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FrisbeeApp.Controllers.Controllers
{
    [ApiController]
    [Authorize(Roles = "Admin")]
    [Route("api/[controller]")]
    public class AdminController : ControllerBase
    {
        private readonly IAdminRepository _repository;
        private readonly IMapper _mapper;
        
        public AdminController(IAdminRepository repository, IMapper mapper) 
        {
            _repository = repository;  
            _mapper = mapper;   
        }

        [HttpPost]
        [Route("create-team")]
        public async Task<bool> CreateTeam(string teamName)
        {
            return await _repository.CreateTeam(teamName);
        }
    }
}
