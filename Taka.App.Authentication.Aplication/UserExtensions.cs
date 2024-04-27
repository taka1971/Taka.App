﻿using Taka.App.Authentication.Domain.Dtos;
using Taka.App.Authentication.Domain.Entities;

namespace Taka.App.Authentication.Application
{
    public static class UserExtensions
    {
        public static UserResponseDto ToDto(this User user)
        {
            return new UserResponseDto(user.Id, user.Email, user.AccessibleMicroservices);            
        }
    }
}
