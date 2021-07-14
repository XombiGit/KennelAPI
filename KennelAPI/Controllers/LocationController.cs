using Common.Interfaces;
using KennelAPI.Controllers.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KennelAPI.Controllers
{
    [Route("api/location")]
    public class LocationController : Controller
    {
        private readonly ILocationRepository _locationRepository;

        public LocationController(ILocationRepository locationRepository)
        {
            _locationRepository = locationRepository;
        }

        [Authorize]
        [HttpGet("all/{dogId}")]
        public async Task<IActionResult> GetAllNearbyDogs(string dogId)
        {
            var userId = ControllerHelper.getUserFromToken(Request);

            if (userId == null)
            {
                return Unauthorized(); //Create a test check this one, how ?
            }

            //TODO: prevent a user from accessing a dog that they don't own

            var dogs = await _locationRepository.GetNearbyDogs(dogId);

            if (dogs == null)
            {
                return NotFound();
            }

            for (int i = 0; i < dogs.Count(); i++)
            {
                /*if (dogs[i].OwnerID != userId.Value)
                {
                    return Unauthorized();
                }*/
            }
            return Ok(dogs);
        }
    }
}
