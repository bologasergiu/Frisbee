﻿using AutoMapper;
using Frisbee.ApiModels;
using FrisbeeApp.DatabaseModels.Models;
using FrisbeeApp.Logic.Abstractions;
using FrisbeeApp.Logic.Abstractisations;
using FrisbeeApp.Logic.DtoModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FrisbeeApp.Controllers.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IAuthRepository _authRepository;
        private readonly IUserRepository _userRepository;

        public UserController(IMapper mapper, IAuthRepository repository, IUserRepository userRepository)
        {
            _mapper = mapper;
            _authRepository = repository;
            _userRepository = userRepository;
        }

        [AllowAnonymous]
        [Route("register")]
        [HttpPost]
        public async Task<bool> Register(RegisterApiModel registerApiModel)
        {
            var registerUser = _mapper.Map<User>(registerApiModel);

            return await _authRepository.Register(registerUser, registerApiModel.Password, registerApiModel.Role);
        }

        [AllowAnonymous]
        [Route("login")]
        [HttpPost]
        public async Task<TokenDTO> Login(LoginApiModel loginApiModel)
        {
            var loginUser = _mapper.Map<LoginDTO>(loginApiModel);

            return await _authRepository.Login(loginUser);
        }

        [Authorize]
        [Route("logout")]
        [HttpPost]
        public async Task Logout()
        {
            await _authRepository.Logout();
        }
        
        [Authorize(Roles = "Coach, Admin")]
        [Route("approve-account")]
        [HttpPut]
        public async Task<bool> ApproveAccount(Guid id)
        {
            return await _authRepository.ApproveAccount(id, User.Identity.Name);
        }
        
        [Authorize]
        [Route("view-team")]
        [HttpGet]
        public async Task<List<TeamMemberDTO>> ViewTeam(string teamName)
        { 
            return await _userRepository.ViewTeam(teamName);
        }
        
        [Authorize]
        [Route("update-user")]
        [HttpPut]
        public async Task<bool> UpdateUser(Guid Id, UpdateUserApiModel updateUserApiModel)
        {
            var user = _mapper.Map<User>(updateUserApiModel);
            return await _userRepository.UpdateUser(Id, user);
        }

        [Authorize(Roles ="Admin, Coach")]
        [Route("update-team")]
        [HttpPut]
        public async Task<bool> UpdateTeam(Guid Id, string team)
        {
            return await _userRepository.UpdateTeam(Id, team);
        }
        
    };
}
