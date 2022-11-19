using infrastructure_service.Models;

namespace infrastructure_service.Interfaces;

public interface IServiceRepository
{
    public Task<Service> Get(long id);
    public Task<bool> Update(Service service);
    public Task<bool> Delete(long id);
    public Task<long> Create(Service service);
}