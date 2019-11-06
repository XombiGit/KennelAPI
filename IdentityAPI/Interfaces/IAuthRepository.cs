using IdentityAPI.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityAPI.Interfaces
{
    public interface IAuthRepository
    {
        Task<IdentityResult> RegisterUser(UserModel userModel);
    }
}
