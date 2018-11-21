using KennelAPI.Interfaces;
using KennelAPI.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KennelAPI.Controllers
{
    [Route("api/dogs")]
    public class DogController : Controller
    {
        private readonly IDogRepository _dogRepository;

        public DogController(IDogRepository dogRepository)
        {
            _dogRepository = dogRepository;
        }

        [HttpGet("{dogId}")]
        public IActionResult GetDog(int dogId)
        {
            var dog = _dogRepository.GetDog(dogId);

            if (dog == null)
            {
                return NotFound();
            }

            return Ok(dog);
        }

        [HttpDelete("{dogId}")]
        public IActionResult DeleteDog(int dogId)
        {
            if (dogId == null)
            {
                return BadRequest();
            }

            var dogToDelete = _dogRepository.GetDog(dogId);

            if (dogToDelete == null)
            {
                return NotFound();
            }

            _dogRepository.DeleteDog(dogToDelete);

            return NoContent();
        }

        [HttpPost()]
        public IActionResult PostDog([FromBody] DogDtoCreation dogDtoCreation)
        {
            if (dogDtoCreation == null)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                _dogRepository.AddDog(dogDtoCreation.Name, dogDtoCreation.Breed, dogDtoCreation.Phone, 
                                      dogDtoCreation.Email, dogDtoCreation.SpecialNotes, dogDtoCreation.XCoord, 
                                      dogDtoCreation.YCoord, dogDtoCreation.Reward, dogDtoCreation.ImageURL);
            }
            catch (Exception)
            {
                return StatusCode(500, "500 Bad Request");
            }

            return Ok(dogDtoCreation);
        }

        [HttpPut("{dogId}")]
        public IActionResult PutDog(int dogId, [FromBody] DogDtoUpdate dogDtoUpdate)
        {
            if (dogId == 0 || dogDtoUpdate == null)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var dogToUpdate = _dogRepository.GetDog(dogId).Clone();

            if (dogToUpdate == null)
            {
                return NotFound();
            }

            if (dogDtoUpdate.Email != null)
            {
                dogToUpdate.Email = dogDtoUpdate.Email;
            }

            if (dogDtoUpdate.Breed != null)
            {
                dogToUpdate.Breed = dogDtoUpdate.Breed;
            }

            if (dogDtoUpdate.Name != null)
            {
                dogToUpdate.Name = dogDtoUpdate.Name;
            }

            if (dogDtoUpdate.Phone != null)
            {
                dogToUpdate.Phone = dogDtoUpdate.Phone;
            }

            if (dogDtoUpdate.SpecialNotes != null)
            {
                dogToUpdate.SpecialNotes = dogDtoUpdate.SpecialNotes;
            }

            if (dogDtoUpdate.XCoord != 0)
            {
                dogToUpdate.XCoord = dogDtoUpdate.XCoord;
            }

            if (dogDtoUpdate.YCoord != 0)
            {
                dogToUpdate.YCoord = dogDtoUpdate.YCoord;
            }

            if (dogDtoUpdate.Reward != 0)
            {
                dogToUpdate.Reward = dogDtoUpdate.Reward;
            }

            if (dogDtoUpdate.ImageURL != null)
            {
                dogToUpdate.ImageURL = dogDtoUpdate.ImageURL;
            }


            _dogRepository.UpdateDog(dogToUpdate);
            return NoContent();
        }
    }
}
