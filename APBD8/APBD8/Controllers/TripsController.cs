using APBD8.Context;
using APBD8.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace APBD8.Controllers;

[ApiController]
[Route("api")]
public class TripsController : ControllerBase {
    private readonly Apbd8Context dbContext;

    public TripsController(Apbd8Context dbContext) {
        this.dbContext = dbContext;
    }


    [HttpGet]
    [Route("trips")]
    public IActionResult GetTrips([FromQuery] int? page, [FromQuery] int? pageSize = 10) {
        var defaultPageSize = 10;

        var result = dbContext.Trips
            .Select(e => new {
                e.Name,
                e.Description,
                e.DateFrom,
                e.DateTo,
                e.MaxPeople,
                Countries = e.IdCountries.Select(c => new { c.Name }),
                Clients =
                    e.ClientTrips.Join(dbContext.Clients,
                        ct => ct.IdClient,
                        c => c.IdClient,
                        (ct, c) => new {
                            c.FirstName, c.LastName
                        })
            }).OrderBy(e => e.DateFrom);
        if (page.HasValue) {
            page = int.Max((int)page, 1);
            var actualPageSize = pageSize ?? defaultPageSize;
            actualPageSize = actualPageSize < 1 ? defaultPageSize : actualPageSize;

            var skipRows = ((int)page - 1) * actualPageSize;
            var totalPages = result.Count();
            var pagedResult = result.Skip(skipRows).Take(actualPageSize);
            return Ok(new {
                Page = page,
                PageSize = pageSize,
                AllPages = Math.Ceiling((double)(totalPages / pageSize)!),
                Trips = pagedResult
            });
        }


        return Ok(result);
    }


    [HttpDelete]
    [Route("clients")]
    public IActionResult DeleteClient([FromRoute] int idClient) {
        var exists = dbContext.Clients.Any(e => e.IdClient == idClient);
        if (!exists)
            return NotFound("No client found with given id");

        var hasTrips = dbContext.ClientTrips.Any(e => e.IdClient == idClient);
        if (hasTrips) {
            return NotFound("Client with given Id has trips registered, so they cannot be deleted");
        }

        dbContext.Clients.Remove(dbContext.Clients.FirstOrDefault(e => e.IdClient == idClient)!);
        dbContext.SaveChanges();
        return Ok("Client removed");
    }


    [HttpPost]
    [Route("trips/{idTrip:int}/clients")]
    public IActionResult AddClientToTrip([FromBody] AddClientToTrip clientToAdd,[FromRoute] int idTrip) {
        if (clientToAdd.IdTrip != idTrip)
            return BadRequest("Query idTrip and given idTrip dont match up");
        
        var alreadyExists = dbContext.Clients.Any(e => e.Pesel == clientToAdd.Pesel);
        if (alreadyExists)
            return BadRequest("A client with given PESEL already exists");

        var tripExists = dbContext.Trips
            .Any(e =>
                e.IdTrip == clientToAdd.IdTrip
                &&
                e.Name == clientToAdd.TripName);
        if (!tripExists)
            return BadRequest("given trip does not exist");

        var alreadyHappened = dbContext.Trips
            .First(e => e.IdTrip == clientToAdd.IdTrip)
            .DateFrom < DateTime.Now;
        if (alreadyHappened)
            return BadRequest("given trip has already happened");

        var alreadyAddedToTrip = dbContext.ClientTrips
            .Where(e => e.IdTrip == clientToAdd.IdTrip)
            .GroupJoin(dbContext.Clients,
                ct => ct.IdClient,
                c => c.IdClient,
                (trip, clients) => new {
                    trip,
                    clients
                }).First()
            .clients.Any(e => e.Pesel == clientToAdd.Pesel);

        if (alreadyAddedToTrip)
            return BadRequest("A client with given PESEL is already a part of given trip");

        using (var transaction = dbContext.Database.BeginTransaction()) {
            try {
                var newClient = new Client {
                    FirstName = clientToAdd.FirstName,
                    LastName = clientToAdd.LastName,
                    Email = clientToAdd.Email,
                    Telephone = clientToAdd.Telephone,
                    Pesel = clientToAdd.Pesel
                };

                dbContext.Clients.Add(newClient);
                dbContext.SaveChanges();  

                dbContext.ClientTrips.Add(new ClientTrip {
                    IdClient = newClient.IdClient,
                    IdTrip = clientToAdd.IdTrip,
                    PaymentDate = clientToAdd.PaymentDate,
                    RegisteredAt = DateTime.Now
                });

                dbContext.SaveChanges();
                transaction.Commit();
                return StatusCode(201,"new client with id created:" + newClient.IdClient);
            } catch (Exception ex) {
                transaction.Rollback(); 
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}