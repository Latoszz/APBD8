using APBD8.Models;
using APBD8.Repositories;
using APBD8.Services;


namespace APBD8.Services;



using APBD8.Models;

public interface ITripService
{
    object GetTrips(int? page, int? pageSize);
}
    
public class TripService : ITripService
{
    private readonly ITripRepository _tripRepository;

    public TripService(ITripRepository tripRepository)
    {
        _tripRepository = tripRepository;
    }

    public object GetTrips(int? page, int? pageSize)
    {
        var defaultPageSize = 10;
        var trips = _tripRepository.GetTripsWithDetails();

        if (page.HasValue)
        {
            page = int.Max((int)page, 1);
            var actualPageSize = pageSize ?? defaultPageSize;
            actualPageSize = actualPageSize < 1 ? defaultPageSize : actualPageSize;

            var skipRows = ((int)page - 1) * actualPageSize;
            var totalCount = trips.Count();
            var pagedResult = trips.Skip(skipRows).Take(actualPageSize);
            
            return new {
                Page = page,
                PageSize = pageSize,
                AllPages = Math.Ceiling((double)(totalCount / actualPageSize)),
                Trips = pagedResult
            };
        }

        return trips;
    }
}


