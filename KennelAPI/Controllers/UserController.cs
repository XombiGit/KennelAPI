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
            var bearerToken = Request.Headers["Authorization"];
            var actualToken = bearerToken.FirstOrDefault();
            var tokenSplit = actualToken?.Split(' '); //nullable operator

            if (tokenSplit == null || tokenSplit.Length < 2)
            {
                //how to check for length < 2
                return new UnauthorizedObjectResult(null);
            }
            var jwtToken = new JwtSecurityToken(actualToken.Split(' ')[1]);
            var claims = jwtToken.Claims;
            var userIdFromToken = claims.FirstOrDefault(c => c.Type == "userId");

            if (userIdFromToken == null || userIdFromToken.Value != userId)
            {
                return new UnauthorizedObjectResult(null);
            }

            var user = await _userRepository.GetUser(userId);

            if (user == null)
            {
                return NotFound();
            }

            return Ok(user);
        }

        [HttpPost()]
        public async Task<IActionResult> AddUser([FromBody] UserDtoCreation userDtoCreation)
        {
            //how to setup token
            if (userDtoCreation == null)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            IUserEntity userEntity;

            try
            {
                userEntity = Mapper.Map<UserEntity>(userDtoCreation);
                userEntity.UserID = Guid.NewGuid().ToString();
                await _userRepository.AddUser(userEntity);
            }
            catch (Exception)
            {
                return StatusCode(500, "500 Internal Server Error");
            }

            return Ok(userDtoCreation);
        }

        [HttpPut("{userId}")]
        public async Task<IActionResult> UpdateUsers(string userId, [FromBody] UserDtoUpdate userDtoUpdate)
        {
            var bearerToken = Request.Headers["Authorization"];
            var actualToken = bearerToken.FirstOrDefault();
            var tokenSplit = actualToken?.Split(' '); //nullable operator

            if (tokenSplit == null || tokenSplit.Length < 2)
            {
                //how to check for length < 2
                return new UnauthorizedObjectResult(null);
            }
            var jwtToken = new JwtSecurityToken(actualToken.Split(' ')[1]);
            var claims = jwtToken.Claims;
            var userIdFromToken = claims.FirstOrDefault(c => c.Type == "userId");

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

            if (userDtoUpdate.Email != null)
            {
                userToUpdate.Email = userDtoUpdate.Email;
            }
            
            if(userDtoUpdate.Name != null)
            {
                userToUpdate.Name = userDtoUpdate.Name;
            }

            if(userDtoUpdate.Phone != null)
            {
                userToUpdate.Phone = userDtoUpdate.Phone;
            }

            _userRepository.UpdateUser(userToUpdate);
            return NoContent();
        }

        [HttpDelete("{userId}")]
        public async Task<IActionResult> DeleteUser(string userId)
        {
            var bearerToken = Request.Headers["Authorization"];
            var actualToken = bearerToken.FirstOrDefault();
            var tokenSplit = actualToken?.Split(' '); //nullable operator

            if (tokenSplit == null || tokenSplit.Length < 2)
            {
                //how to check for length < 2
                return new UnauthorizedObjectResult(null);
            }
            var jwtToken = new JwtSecurityToken(actualToken.Split(' ')[1]);
            var claims = jwtToken.Claims;
            var userIdFromToken = claims.FirstOrDefault(c => c.Type == "userId");

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
