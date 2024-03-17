﻿using Application.Services.User.Requests;
using Common.Core.DependencyInjection;
using Domain.User.Repositories;
using Infra.Core.ApplicationBasics;

namespace Application.Services.User
{
    [ServiceLocate(typeof(IUserService))]
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<ApplicationResult> Register(RegisterUserRequest request)
        {
            var user = new Domain.User.Entities.User
            {
                UserName = request.UserName,
                DisplayName = request.UserName,
                PasswordHash = request.PasswordHash
            };
            
            var result = await _userRepository.Add(user);

            return new ApplicationResult
            {
                Message = result.Message,
                Success = result.Success
            };
        }
    }
}
