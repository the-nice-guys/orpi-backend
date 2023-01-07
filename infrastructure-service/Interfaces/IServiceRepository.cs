using OrpiLibrary.Models;

namespace infrastructure_service.Interfaces;

public interface IServiceRepository
{
    public Task<Service> Get(long id);
    public Task<bool> InsertHostService(long hostId, long serviceId);
    public Task<IEnumerable<Service>> GetAllForInfrastructure(long infrastructureId);
    public Task<bool> Update(Service service);
    public Task<bool> Delete(long id);
    public Task<long> Create(Service service);
}