using OrpiLibrary.Models;

namespace coordinator_service.Interfaces;

public interface IDeploymentService
{
    public Task<bool> DeployServicesQueuesAsync(IEnumerable<IEnumerable<Service>> servicesQueues,
        CancellationToken cancellationToken);
}