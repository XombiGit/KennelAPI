using AutoMapper;
using Common.Entities;
using Common.Interfaces;
using Common.Interfaces.Services;
using KennelAPI.Models;
using KennelAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MongoPersistence.Entities;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace KennelAPI.Controllers
{
    [Route("api/dogs")]
    public class DogController : Controller
    {
        private readonly IDogRepository _dogRepository;
        private readonly IMailService _mailService;

        public DogController(IDogRepository dogRepository, IMailService mailService)
        {
            _dogRepository = dogRepository;
            _mailService = mailService;
        }

        [Authorize]
        [HttpGet("{dogId}")]
        public async Task<IActionResult> GetDog(string dogId)
        {
            var bearerToken = Request.Headers["Authorization"];
            var actualToken = bearerToken.FirstOrDefault();
            var tokenSplit = actualToken?.Split(' '); //nullable operator

            if (tokenSplit == null || tokenSplit.Length < 2)
            {
                return Unauthorized();
            }
            var jwtToken = new JwtSecurityToken(actualToken.Split(' ')[1]);
            var claims = jwtToken.Claims;
            var userId = claims.FirstOrDefault(c => c.Type == "userId");
            
            if(userId == null)
            {
                return Unauthorized(); //Create a test check this one, how ?
            }

            var dog = await _dogRepository.GetDog(dogId);

            if (dog == null)
            {
                return NotFound();
            }

            if(dog.OwnerID != userId.Value)
            {
                return Unauthorized();
            }

            return Ok(dog);
        }

        [HttpDelete("{dogId}")]
        public async Task<IActionResult> DeleteDog(string dogId)
        {
            var bearerToken = Request.Headers["Authorization"];
            var actualToken = bearerToken.FirstOrDefault();
            var jwtToken = new JwtSecurityToken(actualToken.Split(' ')[1]);

            var claims = jwtToken.Claims;
            var userId = claims.FirstOrDefault(c => c.Type == "userId");

            if (userId == null)
            {
                return Unauthorized(); //Create a test check this one
            }

            if (dogId == null)
            {
                return BadRequest();
            }

            var dogToDelete = await _dogRepository.GetDog(dogId);

            if (dogToDelete == null)
            {
                return NotFound();
            }

            if (dogToDelete.OwnerID != userId.Value)
            {
                return Unauthorized();
            }

            //after removing dog, it goes to Dispose, why ?
            await _dogRepository.DeleteDog(dogToDelete);
         
            return NoContent();
            //TODO
            //_mailService.SendMail("hello", "world");
        }

        [HttpPost()]
        public async Task<IActionResult> PostDog([FromBody] DogDtoCreation dogDtoCreation)
        {
            var bearerToken = Request.Headers["Authorization"];
            var actualToken = bearerToken.FirstOrDefault();
            var jwtToken = new JwtSecurityToken(actualToken.Split(' ')[1]);

            var claims = jwtToken.Claims;
            var userId = claims.FirstOrDefault(c => c.Type == "userId");

            if (userId == null)
            {
                return Unauthorized(); //Create a test check this one
            }

            if (dogDtoCreation == null)
            {
                return BadRequest();
            }
            //If I test once in another method, do i need to test in another as well ?
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            IDogEntity dogEntity;

            try
            {
                //how to link dogID with userID, pass userID in from ?
                dogEntity = Mapper.Map<DogEntity>(dogDtoCreation);
                dogEntity.DogID = Guid.NewGuid().ToString();

                await _dogRepository.AddDog(dogEntity);
            }
            catch (Exception ex)
            {
                //How to mock this ?
                return StatusCode(500, "500 Internal Server Error");
            }

            return Ok(dogEntity);
        }

        [HttpPut("{dogId}")]
        public async Task<ActionResult> PutDog(string dogId, [FromBody] DogDtoUpdate dogDtoUpdate)
        {
            var bearerToken = Request.Headers["Authorization"];
            var actualToken = bearerToken.FirstOrDefault();
            var jwtToken = new JwtSecurityToken(actualToken.Split(' ')[1]);

            var claims = jwtToken.Claims;
            var userId = claims.FirstOrDefault(c => c.Type == "userId");

            if (userId == null)
            {
                return Unauthorized(); //Create a test check this one
            }

            if (dogId == null || dogDtoUpdate == null)
            {
                return BadRequest();
            }

            //ModelState can't truly be tested since I'm setting the errors for the model state
            if (!ModelState.IsValid)
            {
                //return BadRequest(ModelState); returns object
                return BadRequest();
            }

            var aDog = await _dogRepository.GetDog(dogId);
            var dogEntity = aDog?.Clone();

            if (dogEntity == null)
            {
                return NotFound();
            }

            if (dogEntity.OwnerID != userId.Value)
            {
                return Unauthorized();
            }

            if (dogDtoUpdate.Email != null)
            {
                dogEntity.Email = dogDtoUpdate.Email;
            }

            if (dogDtoUpdate.Breed != null)
            {
                dogEntity.Breed = dogDtoUpdate.Breed;
            }

            if (dogDtoUpdate.Name != null)
            {
                dogEntity.Name = dogDtoUpdate.Name;
            }

            if (dogDtoUpdate.Phone != null)
            {
                dogEntity.Phone = dogDtoUpdate.Phone;
            }

            if (dogDtoUpdate.SpecialNotes != null)
            {
                dogEntity.SpecialNotes = dogDtoUpdate.SpecialNotes;
            }

            if (dogDtoUpdate.XCoord != 0)
            {
                dogEntity.XCoord = dogDtoUpdate.XCoord;
            }

            if (dogDtoUpdate.YCoord != 0)
            {
                dogEntity.YCoord = dogDtoUpdate.YCoord;
            }

            if (dogDtoUpdate.Reward != 0)
            {
                dogEntity.Reward = dogDtoUpdate.Reward;
            }

            if (dogDtoUpdate.ImageURL != null)
            {
                dogEntity.ImageURL = dogDtoUpdate.ImageURL;
            }


            _dogRepository.UpdateDog(dogEntity);
            return NoContent();
        }
    }
}
