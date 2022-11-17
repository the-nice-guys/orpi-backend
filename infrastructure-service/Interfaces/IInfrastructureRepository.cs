using infrastructure_service.Models;

namespace infrastructure_service.Interfaces;

public interface IInfrastructureRepository
{
    Task<long> Create(Infrastructure infrastructure);
    Task<bool> Update(Infrastructure infrastructure);
    Task<bool> Delete(long id);
    Task<Infrastructure> Get(long id);
    Task<IEnumerable<Infrastructure>> GetAllForUser(long userId);
}