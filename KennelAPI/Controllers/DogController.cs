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

        [HttpGet("{dogId}")]
        //[Authorize("Bearer")]
        public async Task<IActionResult> GetDog(string dogId)
        {
            /*var identity = (ClaimsIdentity)User.Identity;
            IEnumerable<Claim> claims = identity.Claims;

            var x = User.Claims;*/
            //Add token check to all other functions Post, Delete, Put and test as well
            var bearerToken = Request.Headers["Authorization"];
            var actualToken = bearerToken.FirstOrDefault();
            var jwtToken = new JwtSecurityToken(actualToken.Split(' ')[1]);

            var claims = jwtToken.Claims;
            var userId = claims.FirstOrDefault(c => c.Type == "userId");
            
            if(userId == null)
            {
                return Unauthorized(); //Create a test check this one
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
            if (dogId == null)
            {
                return BadRequest();
            }

            var dogToDelete = await _dogRepository.GetDog(dogId);

            if (dogToDelete == null)
            {
                return NotFound();
            }

            await _dogRepository.DeleteDog(dogToDelete);
            
            //TODO
            _mailService.SendMail("hello", "world");

            return NoContent();
        }

        [HttpPost()]
        public async Task<IActionResult> PostDog([FromBody] DogDtoCreation dogDtoCreation)
        { 

            if (dogDtoCreation == null)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            IDogEntity dogEntity;

            try
            {
                dogEntity = Mapper.Map<DogEntity>(dogDtoCreation);
                dogEntity.DogID = Guid.NewGuid().ToString();

                await _dogRepository.AddDog(dogEntity);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "500 Internal Server Error");
            }

            return Ok(dogEntity);
        }

        [HttpPut("{dogId}")]
        public async Task<ActionResult> PutDog(string dogId, [FromBody] DogDtoUpdate dogDtoUpdate)
        {
            if (dogId == null || dogDtoUpdate == null)
            {
                return BadRequest();
            }

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
