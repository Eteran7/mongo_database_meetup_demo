using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

using ParkFinder.Models;
using ParkFinder.Services;

namespace ParkFinder.Controllers
{
    [Route("api/park")]
    [ApiController]
    public class ParkController : ControllerBase
    {
        private readonly IParkService _service;

        public ParkController(IParkService service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task<ActionResult<ParkModel>> Create([FromBody]ParkModel park)
        {
            var result = await _service.Create(park);
            if (result == null)
            {
                return BadRequest("Failed to create new park!");
            }
            else
            {
                return CreatedAtAction(nameof(Retrieve), new { guid = park.Id }, result);
            }
        }

        [HttpGet]
        public async Task<ActionResult<ParkModel>> Retrieve([FromQuery]Guid guid)
        {
            var result = await _service.Retrieve(guid);

            if (result == null)
                return NotFound();
            else
                return Ok(result);
        }

        [HttpPut]
        public async Task<ActionResult<ParkModel>> Update([FromQuery]Guid guid, [FromBody]ParkModel park)
        {
            var result = await _service.Update(guid, park);

            if (result == null)
                return NotFound();
            else
                return Ok(result);
        }

        [HttpDelete]
        public async Task<ActionResult<ParkModel>> Delete([FromQuery]Guid guid)
        {
            var result = await _service.Delete(guid);

            if (result == null)
                return NotFound();
            else
                return NoContent();
        }

        [HttpGet]
        [Route("near")]
        public async Task<ActionResult<ParkModel>> FindNear([FromQuery]double longitude, [FromQuery]double latitude, [FromQuery]double threshold)
        {
            var results = await _service.FindNear(longitude, latitude, threshold);

            if (results == null)
                return NotFound();
            else
                return Ok(results);
        }
    }
}
