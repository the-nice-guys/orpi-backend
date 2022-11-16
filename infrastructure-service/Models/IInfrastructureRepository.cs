namespace infrastructure_service.Models;

public interface IInfrastructureRepository
{
    Task Create(Infrastructure infrastructure);
    Task Update(Infrastructure infrastructure);
    Task Delete(Infrastructure infrastructure);
    Task<Infrastructure> Get(long id);
    Task<IEnumerable<Infrastructure>> GetAllForUser(long userId);
}