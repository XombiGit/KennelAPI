using IdentityAPI.Interfaces;
using IdentityAPI.Models;
using IdentityAPI.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityAPI.Controllers
{
    [Route("/accounts")]
    public class AccountsController : Controller
    {
        private IAuthRepository _authRepository = null;

        public AccountsController(IAuthRepository repo)
        {
            _authRepository = repo;
        }

        [HttpPost("Authenticate")]
        //TODO: Test this method
        public async Task<IActionResult> Authenticate([FromBody]UserModel userParam)
        {
            var user = await _authRepository.Authenticate(userParam);

            if (user == null)
                return BadRequest(new { message = "Username or password is incorrect" });

            return Ok(user);
        }

        [HttpPost("Signup")]
        public async Task<IActionResult> SignupAsync([FromBody]UserModel userParam)
        {
            //validate
            var result = await _authRepository.GetUserByNameAsync(userParam);
            if (result != null)
            {
                return StatusCode(409, "409 Username already exists");
            }
            //create user
            userParam.UserId = Guid.NewGuid().ToString();
            var user = await _authRepository.Signup(userParam);

            return Ok(user);
        }
        
        private IActionResult GetErrorResult(IdentityResult result)
        {
            if (result == null)
            {
                return StatusCode(500, "500 Internal Server Error");
            }

            if (!result.Succeeded)
            {
                if (result.Errors != null)
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                }

                if (ModelState.IsValid)
                {
                    // No ModelState errors are available to send, so just return an empty BadRequest.
                    return BadRequest();
                }

                return BadRequest(ModelState);
            }

            return null;
        }
    }
}
