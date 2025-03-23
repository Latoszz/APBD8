using APBD8.Models;
using APBD8.Services;
using Microsoft.AspNetCore.Mvc;

namespace APBD8.Controllers;

[ApiController]
[Route("api")]
public class TripsController : ControllerBase {
    private readonly ITripService _tripService;
    private readonly IClientService _clientService;

    public TripsController(ITripService tripService, IClientService clientService) {
        _tripService = tripService;
        _clientService = clientService;
    }

    [HttpGet]
    [Route("trips")]
    public IActionResult GetTrips([FromQuery] int? page, [FromQuery] int? pageSize = 10) {
        var result = _tripService.GetTrips(page, pageSize);
        return Ok(result);
    }

    [HttpDelete]
    [Route("clients")]
    public IActionResult DeleteClient([FromRoute] int idClient) {
        try {
            _clientService.DeleteClient(idClient);
            return Ok("Client removed");
        }
        catch (KeyNotFoundException ex) {
            return NotFound(ex.Message);
        }
        catch (InvalidOperationException ex) {
            return BadRequest(ex.Message);
        }
    }

    [HttpPost]
    [Route("trips/{idTrip:int}/clients")]
    public IActionResult AddClientToTrip([FromBody] AddClientToTrip clientToAdd, [FromRoute] int idTrip) {
        if (clientToAdd.IdTrip != idTrip)
            return BadRequest("Query idTrip and given idTrip don't match up");
        
        try {
            var newClientId = _clientService.AddClientToTrip(clientToAdd);
            return StatusCode(201, "new client with id created:" + newClientId);
        }
        catch (BadHttpRequestException ex) {
            return BadRequest(ex.Message);
        }
        catch (Exception ex) {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }
}