using KennelAPI.Interfaces;
using KennelAPI.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
        public IActionResult GetUser(int userId)
        {
            var user = _userRepository.GetUser(userId);

            if (user == null)
            {
                return NotFound();
            }

            return Ok(user);
        }

        [HttpPost()]
        public IActionResult AddUser([FromBody] UserDtoCreation userDtoCreation)
        {
            if (userDtoCreation == null)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                _userRepository.AddUser(userDtoCreation.Name, userDtoCreation.Phone, userDtoCreation.Email);
            }
            catch (Exception)
            {
                return StatusCode(500, "500 Bad Request");
            }

            return Ok(userDtoCreation);
        }

        [HttpPut("{userId}")]
        public IActionResult UpdateUsers(int userId, [FromBody] UserDtoUpdate userDtoUpdate)
        {
            if (userId == null || userDtoUpdate == null)
            {
                return BadRequest();
            }

            if(!ModelState.IsValid)
            {
                return BadRequest();
            }

            var userToUpdate = _userRepository.GetUser(userId).Clone();

            if(userToUpdate == null)
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
        public IActionResult DeleteUser(int userId)
        {
            if(userId == null)
            {
                return BadRequest();
            }

            var userToDelete =_userRepository.GetUser(userId);

            if(userToDelete == null)
            {
                return NotFound();
            }

            _userRepository.DeleteUser(userToDelete);

            return NoContent();
        }
    }
}
