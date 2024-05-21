using APBD8.Context;
using APBD8.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace APBD8.Controllers;

[ApiController]
[Route("api/trips")]
public class TripsController : ControllerBase {
    //private readonly IConfiguration configuration;
    private readonly Apbd8Context dbContext;

    public TripsController(Apbd8Context dbContext) {
        this.dbContext = dbContext;
    }

    [HttpGet]
    public IActionResult GetTrips() {
        //var tmp = dbContext.Trips;
        
        /*.Select(e => new {
            e.Name,
            e.Description,
            e.DateFrom,
            e.DateTo,
            Countries = e.IdCountries.Select(c => c.Name),
            Clients =
                e.ClientTrips.Join(dbContext.Clients,
                ct => ct.IdClient,
                c => c.IdClient,
                (ct, c) => new {
                    c.FirstName,
                    c.LastName
                })
        });*/
        return Ok(1);
    }
}