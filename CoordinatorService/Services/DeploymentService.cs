using System.Text.Json;
using coordinator_service.Interfaces;
using coordinator_service.Models;
using Microsoft.Extensions.Caching.Distributed;
using OrpiLibrary.Models;
using OrpiLibrary.Models.Common;
using OrpiLibrary.Models.Docker.Enums;

namespace coordinator_service.Services;

public class DeploymentService: IDeploymentService
{
    private IDistributedCache _cache;
    private IProducerService _producerService;
    
    public DeploymentService(IDistributedCache cache, IProducerService producerService)
    {
        _cache = cache;
        _producerService = producerService;
    }
    
    public async Task<bool> DeployServicesQueuesAsync(IEnumerable<IEnumerable<Service>> servicesQueues, CancellationToken cancellationToken)
    {
        CancellationTokenSource cancellationTokenSource = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
        List<Task> tasks = new();
        
        var queues = servicesQueues as IEnumerable<Service>[] ?? servicesQueues.ToArray();
        foreach (var servicesQueue in queues)
        {
            tasks.Add(Task.Run(
                () => DeployServicesQueueAsync(servicesQueue, cancellationTokenSource), 
                cancellationTokenSource.Token));
        }

        try
        {
            await Task.WhenAll(tasks);
            await ClearCacheAsync(queues);
            return true;
        }
        catch (OperationCanceledException e)
        {
            Console.WriteLine(e);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            //cancellationTokenSource.Cancel(false);
        }
        return false;
    }
    
    private async Task DeployServicesQueueAsync(IEnumerable<Service> servicesQueue, CancellationTokenSource cts)
    {
        foreach (var service in servicesQueue)
        {
            if (cts.IsCancellationRequested)
            {
                Console.WriteLine("Cancellation requested");
                return;
            }
            
            Request<DockerRequestType> request = new()
            {
                Type = DockerRequestType.Create,
                Guid = Guid.NewGuid(),
                Payload = JsonSerializer.Serialize(service)
            };

            await _producerService.Produce("docker-requests", JsonSerializer.Serialize(request));
            await _cache.SetStringAsync(service.Id.ToString(), "deploy in progress");

            bool isDeployed = false;
            while (!cts.IsCancellationRequested)
            {
                await Task.Delay(1000);
                var status = await _cache.GetStringAsync(service.Id.ToString());
                switch (status)
                {
                    case "deployed":
                        isDeployed = true;
                        break;
                    case "failed":
                        cts.Cancel(false);
                        throw new Exception($"Deployment of service {service.Id} failed");
                }
                
                if (isDeployed)
                {
                    break;
                }
            }
        }
    }
    
    public async Task RollbackDeploymentAsync(IEnumerable<Service> servicesQueue)
    {
        foreach (var service in servicesQueue)
        {
            Request<DockerRequestType> request = new()
            {
                Type = DockerRequestType.Delete,
                Guid = Guid.NewGuid(),
                Payload = JsonSerializer.Serialize(service)
            };

            //await _producerService.Produce("docker-requests", JsonSerializer.Serialize(request));
            await _cache.SetStringAsync(service.Id.ToString(), "rollback in progress");
        }
    }
    
    private async Task ClearCacheAsync(IEnumerable<IEnumerable<Service>> servicesQueues)
    {
        foreach (var servicesQueue in servicesQueues)
        {
            foreach (var service in servicesQueue)
            {
                await _cache.RemoveAsync(service.Id.ToString());
            }
        }
    }
}