using KennelAPI.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Common.Interfaces;
using MongoPersistence.Entities;
using AutoMapper;
using Common.Entities;
using System.IdentityModel.Tokens.Jwt;
using KennelAPI.Controllers.Helpers;

namespace KennelAPI.Controllers
{
    [Route("api/users")]
    public class UserController : Controller
    {
        private readonly IUserRepository _userRepository;

        public UserController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        [HttpGet("{userId}")]
        public async Task<IActionResult> GetUser(string userId)
        {
            var userIdFromToken = ControllerHelper.getUserFromToken(Request);

            if (userIdFromToken == null || userIdFromToken.Value != userId)
            {
                return new UnauthorizedObjectResult(null);
            }

            var user = await _userRepository.GetUser(userId);

            if (user == null)
            {
                return NotFound();
            }

            user.Password = null;
            return Ok(user);
        }

        [HttpPut("{userId}")]
        public async Task<IActionResult> UpdateUsers(string userId, [FromBody] UserDtoUpdate userDtoUpdate)
        {
            var userIdFromToken = ControllerHelper.getUserFromToken(Request);

            if (userIdFromToken == null || userIdFromToken.Value != userId)
            {
                return new UnauthorizedObjectResult(null);
            }

            if (!ModelState.IsValid || userDtoUpdate == null)
            {
                return BadRequest(ModelState);
            }

            /*if(!ModelState.IsValid)
            {
                return BadRequest();
            }*/

            var userToUpdate = await _userRepository.GetUser(userId);
            var userEntity = userToUpdate?.Clone();

            if (userToUpdate == null)
            {
                return NotFound();
            }
            
            if(userDtoUpdate.UserName != null)
            {
                userToUpdate.UserName = userDtoUpdate.UserName;
            }

            if(userDtoUpdate.Password != null)
            {
                userToUpdate.Password = userDtoUpdate.Password;
            }

            _userRepository.UpdateUser(userToUpdate);
            return NoContent();
        }

        //[HttpDelete("{userId}")]
        public async Task<IActionResult> DeleteUser(string userId)
        {
            //TODO add staff type for admin to allow this function to work

            var userIdFromToken = ControllerHelper.getUserFromToken(Request);

            if (userIdFromToken == null || userIdFromToken.Value != userId)
            {
                return new UnauthorizedObjectResult(null);
            }

            if (userId == null)
            {
                return BadRequest(ModelState);
            }

            var userToDelete = await _userRepository.GetUser(userId);

            if(userToDelete == null)
            {
                //return NotFound();
                return new UnauthorizedObjectResult(null);
            }

            _userRepository.DeleteUser(userToDelete);

            return NoContent();
        }
    }
}
