using APBD8.Models;
using APBD8.Repositories;
using APBD8.Services;

namespace APBD8.Services;

public interface IClientService
{
    void DeleteClient(int idClient);
    int AddClientToTrip(AddClientToTrip clientToAdd);
}
public class ClientService : IClientService
{
    private readonly IClientRepository _clientRepository;
    private readonly ITripRepository _tripRepository;

    public ClientService(IClientRepository clientRepository, ITripRepository tripRepository)
    {
        _clientRepository = clientRepository;
        _tripRepository = tripRepository;
    }

    public void DeleteClient(int idClient)
    {
        var clientExists = _clientRepository.ClientExists(idClient);
        if (!clientExists)
            throw new KeyNotFoundException("No client found with given id");

        var hasTrips = _clientRepository.ClientHasTrips(idClient);
        if (hasTrips)
            throw new InvalidOperationException("Client with given Id has trips registered, so they cannot be deleted");

        _clientRepository.DeleteClient(idClient);
    }

    public int AddClientToTrip(AddClientToTrip clientToAdd)
    {
        var alreadyExists = _clientRepository.ClientExistsByPesel(clientToAdd.Pesel);
        if (alreadyExists)
            throw new BadHttpRequestException("A client with given PESEL already exists");

        var tripExists = _tripRepository.TripExistsWithNameAndId(clientToAdd.IdTrip, clientToAdd.TripName);
        if (!tripExists)
            throw new BadHttpRequestException("given trip does not exist");

        var alreadyHappened = _tripRepository.TripHasAlreadyHappened(clientToAdd.IdTrip);
        if (alreadyHappened)
            throw new BadHttpRequestException("given trip has already happened");

        var alreadyAddedToTrip = _clientRepository.ClientWithPeselIsInTrip(clientToAdd.Pesel, clientToAdd.IdTrip);
        if (alreadyAddedToTrip)
            throw new BadHttpRequestException("A client with given PESEL is already a part of given trip");

        return _clientRepository.AddClientToTrip(clientToAdd);
    }
}