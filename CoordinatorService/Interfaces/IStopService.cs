using OrpiLibrary.Models;

namespace coordinator_service.Interfaces;

public interface IStopService
{
    public Task<bool> StopServicesQueuesAsync(IEnumerable<IEnumerable<Service>> servicesQueues,
        CancellationToken cancellationToken);
}