using Microsoft.AspNetCore.Mvc;
using Zadanie9.Services.Interfaces;
using Zadanie9.DTO;

namespace Zadanie9.Controllers
{
    [ApiController]
    [Route("api/trips")]
    public class TripController : ControllerBase
    {
        private readonly ITripService _tripService;
        public TripController(ITripService tripService)
        {
            _tripService = tripService;
        }

        [HttpGet]
        public IActionResult GetTrips([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            var list = _tripService.GetTrips(pageNumber, pageSize);
            return Ok(list);
        }
    }
}