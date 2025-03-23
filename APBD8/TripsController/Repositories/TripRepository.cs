using APBD8.Context;

namespace APBD8.Repositories;

public interface ITripRepository
{
    IQueryable<object> GetTripsWithDetails();
    bool TripExistsWithNameAndId(int idTrip, string tripName);
    bool TripHasAlreadyHappened(int idTrip);
}

public class TripRepository : ITripRepository
{
    private readonly Apbd8Context _dbContext;

    public TripRepository(Apbd8Context dbContext)
    {
        _dbContext = dbContext;
    }

    public IQueryable<object> GetTripsWithDetails()
    {
        return _dbContext.Trips
            .Select(e => new {
                e.Name,
                e.Description,
                e.DateFrom,
                e.DateTo,
                e.MaxPeople,
                Countries = e.IdCountries.Select(c => new { c.Name }),
                Clients =
                    e.ClientTrips.Join(_dbContext.Clients,
                        ct => ct.IdClient,
                        c => c.IdClient,
                        (ct, c) => new {
                            c.FirstName, c.LastName
                        })
            }).OrderBy(e => e.DateFrom);
    }

    public bool TripExistsWithNameAndId(int idTrip, string tripName)
    {
        return _dbContext.Trips.Any(e => e.IdTrip == idTrip && e.Name == tripName);
    }

    public bool TripHasAlreadyHappened(int idTrip)
    {
        return _dbContext.Trips.First(e => e.IdTrip == idTrip).DateFrom < DateTime.Now;
    }
}