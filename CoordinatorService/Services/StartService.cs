using System.Text.Json;
using coordinator_service.Interfaces;
using Docker.DotNet.Models;
using Microsoft.Extensions.Caching.Distributed;
using OrpiLibrary.Models;
using OrpiLibrary.Models.Common;
using OrpiLibrary.Models.Docker.Enums;

namespace coordinator_service.Services;

public class StartService: IStartService
{
    private IDistributedCache _cache;
    private IProducerService _producerService;
    private string _requestTopic;
    
    public StartService(IDistributedCache cache, IProducerService producerService, IConfiguration configuration)
    {
        _cache = cache;
        _producerService = producerService;
        _requestTopic = configuration["Kafka:RequestTopic"];
    }
    
    public async Task<bool> StartServicesQueuesAsync(IEnumerable<IEnumerable<Service>> servicesQueues, CancellationToken cancellationToken)
    {
        CancellationTokenSource cancellationTokenSource = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
        List<Task> tasks = new();
        
        var queues = servicesQueues as IEnumerable<Service>[] ?? servicesQueues.ToArray();
        foreach (var servicesQueue in queues)
        {
            tasks.Add(Task.Run(
                () => StartServicesQueueAsync(servicesQueue, cancellationTokenSource), 
                cancellationTokenSource.Token));
        }

        try
        {
            await Task.WhenAll(tasks);
            // await ClearCacheAsync(queues);
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
            // TODO: Rollback
        }
        return false;
    }
    
    private async Task StartServicesQueueAsync(IEnumerable<Service> servicesQueue, CancellationTokenSource cts)
    {
        foreach (var service in servicesQueue)
        {
            if (cts.IsCancellationRequested)
            {
                Console.WriteLine("Cancellation requested");
                return;
            }

            MakeDockerOptions(service);
            Request<DockerRequest> request = new(
                Guid.NewGuid(),
                DockerRequest.StartContainer,
                JsonSerializer.Serialize(service));

            await _producerService.Produce(_requestTopic, JsonSerializer.Serialize(request));
            await _cache.SetStringAsync(request.Guid.ToString(), "start in progress", new DistributedCacheEntryOptions()
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10)
            });

            bool isStarted = false;
            while (!cts.IsCancellationRequested)
            {
                Console.WriteLine($"Waiting starting of {service.Id}");
                await Task.Delay(1000);
                var status = await _cache.GetStringAsync(request.Guid.ToString());
                switch (status)
                {
                    case "Ok":
                        isStarted = true;
                        break;
                    case "Failed":
                        cts.Cancel(false);
                        throw new Exception($"Starting of service {service.Id} failed");
                }
                
                if (isStarted)
                {
                    break;
                }
            }
        }
    }
    
    public async Task RollbackStartAsync(IEnumerable<Service> servicesQueue)
    {
        foreach (var service in servicesQueue) {
            Request<DockerRequest> request = new(
                Guid.NewGuid(),
                DockerRequest.StopContainer,
                JsonSerializer.Serialize(service));

                //await _producerService.Produce("docker-requests", JsonSerializer.Serialize(request));
            await _cache.SetStringAsync(request.Guid.ToString(), "rollback in progress");
        }
    }

    private void MakeDockerOptions(Service service)
    {
        var options = service.Options.GroupBy(x => x.Name).ToArray();
        service.Options.Clear();
    }
}