using IdentityAPI.Models;
using Microsoft.AspNetCore.Identity;
using MongoPersistence.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityAPI.Interfaces
{
    public interface IAuthRepository
    {
        Task<UserModel> Signup(UserModel userModel);
        Task<UserModel> Authenticate(UserModel userModel);
        Task<UserEntity> GetUserByIdAsync(UserModel userModel);
        Task<UserEntity> GetUserByNameAsync(UserModel userModel);
        UserModel GenerateToken(UserModel userModel);
    }
}
