using Microsoft.AspNetCore.Mvc;
using Zadanie9.Services.Interfaces;
using Zadanie9.DTO;
using System;

namespace Zadanie9.Controllers
{
    [ApiController]
    [Route("api/trips")]
    public class ClientController : ControllerBase
    {
        private readonly IClientService _clientService;
        private readonly ITripService _tripService;

        public ClientController(IClientService clientService, ITripService tripService)
        {
            _clientService = clientService;
            _tripService = tripService;
        }

        [HttpDelete("{idClient}")]
        public IActionResult DeleteClient(int idClient)
        {
            var hasTrips = _tripService.ClientHasTrips(idClient);

            if (hasTrips)
            {
                return BadRequest(new { message = "Client cannot be deleted because they are assigned to one or more trips." });
            }

            var result = _clientService.DeleteClient(idClient);

            if (result)
            {
                return NoContent();
            }
            else
            {
                return NotFound(new { message = "Client not found." });
            }
        }

        [HttpPost("{idTrip}/clients")]
        public IActionResult AssignClientToTrip(int idTrip, [FromBody] ClientAssignmentDTO clientDto)
        {
            // Check if client already exists
            var client = _clientService.GetClientByPesel(clientDto.Pesel);
            if (client != null)
            {
                return BadRequest(new { message = "Client with this PESEL already exists." });
            }

            // Check if client is already assigned to the trip
            if (_tripService.IsClientAssignedToTrip(clientDto.Pesel, idTrip))
            {
                return BadRequest(new { message = "Client is already assigned to this trip." });
            }

            // Check if the trip exists and is in the future
            var trip = _tripService.GetTripById(idTrip);
            if (trip == null)
            {
                return NotFound(new { message = "Trip not found." });
            }

            if (trip.DateFrom <= DateTime.Now)
            {
                return BadRequest(new { message = "Cannot assign client to a trip that has already started or completed." });
            }

            // Register client to the trip
            var registeredAt = DateTime.Now;
            var success = _tripService.AssignClientToTrip(idTrip, clientDto, registeredAt);

            if (success)
            {
                return Ok(new { message = "Client successfully assigned to the trip." });
            }
            else
            {
                return StatusCode(500, new { message = "An error occurred while assigning the client to the trip." });
            }
        }
    }
}
