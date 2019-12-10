using System;
using System.Collections.Generic;
using System.Linq;
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

        [HttpGet]
        [Route("guid")]
        public async Task<ActionResult<List<ParkModel>>> RetrieveByGuids([FromQuery]Guid[] guid)
        {
            var parks = await _service.RetrieveByGuids(guid);

            if (parks.Any())
                return Ok(parks);
            else
                return NotFound();
        }
    }
}
