using OrpiLibrary.Models;

namespace coordinator_service.Interfaces;

public interface IStartService
{
    public Task<bool> StartServicesQueuesAsync(IEnumerable<IEnumerable<Service>> servicesQueues,
        CancellationToken cancellationToken);
}