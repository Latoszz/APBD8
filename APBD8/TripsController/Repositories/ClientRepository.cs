using APBD8.Context;
using APBD8.Models;

namespace APBD8.Repositories;

public interface IClientRepository
{
    bool ClientExists(int idClient);
    bool ClientExistsByPesel(string pesel);
    bool ClientHasTrips(int idClient);
    void DeleteClient(int idClient);
    bool ClientWithPeselIsInTrip(string pesel, int idTrip);
    int AddClientToTrip(AddClientToTrip clientToAdd);
}

public class ClientRepository : IClientRepository {
    private readonly Apbd8Context _dbContext;

    public ClientRepository(Apbd8Context dbContext) {
        _dbContext = dbContext;
    }

    public bool ClientExists(int idClient) {
        return _dbContext.Clients.Any(e => e.IdClient == idClient);
    }

    public bool ClientExistsByPesel(string pesel) {
        return _dbContext.Clients.Any(e => e.Pesel == pesel);
    }

    public bool ClientHasTrips(int idClient) {
        return _dbContext.ClientTrips.Any(e => e.IdClient == idClient);
    }

    public void DeleteClient(int idClient) {
        var client = _dbContext.Clients.FirstOrDefault(e => e.IdClient == idClient);
        if (client != null) {
            _dbContext.Clients.Remove(client);
            _dbContext.SaveChanges();
        }
    }

    public bool ClientWithPeselIsInTrip(string pesel, int idTrip) {
        return _dbContext.ClientTrips
            .Where(e => e.IdTrip == idTrip)
            .GroupJoin(_dbContext.Clients,
                ct => ct.IdClient,
                c => c.IdClient,
                (trip, clients) => new {
                    trip,
                    clients
                }).First()
            .clients.Any(e => e.Pesel == pesel);
    }

    public int AddClientToTrip(AddClientToTrip clientToAdd) {
        using (var transaction = _dbContext.Database.BeginTransaction()) {
            try {
                var newClient = new Client {
                    FirstName = clientToAdd.FirstName,
                    LastName = clientToAdd.LastName,
                    Email = clientToAdd.Email,
                    Telephone = clientToAdd.Telephone,
                    Pesel = clientToAdd.Pesel
                };

                _dbContext.Clients.Add(newClient);
                _dbContext.SaveChanges();

                _dbContext.ClientTrips.Add(new ClientTrip {
                    IdClient = newClient.IdClient,
                    IdTrip = clientToAdd.IdTrip,
                    PaymentDate = clientToAdd.PaymentDate,
                    RegisteredAt = DateTime.Now
                });

                _dbContext.SaveChanges();
                transaction.Commit();
                return newClient.IdClient;
            }
            catch (Exception) {
                transaction.Rollback();
                throw;
            }
        }
    }
}