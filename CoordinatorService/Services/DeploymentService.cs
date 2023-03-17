using System.Text.Json;
using coordinator_service.Interfaces;
using coordinator_service.Models;
using Docker.DotNet.Models;
using Microsoft.Extensions.Caching.Distributed;
using OrpiLibrary.Models;
using OrpiLibrary.Models.Common;
using OrpiLibrary.Models.Docker.Enums;

namespace coordinator_service.Services;

public class DeploymentService: IDeploymentService
{
    private IDistributedCache _cache;
    private IProducerService _producerService;
    private string _requestTopic;
    
    public DeploymentService(IDistributedCache cache, IProducerService producerService, IConfiguration configuration)
    {
        _cache = cache;
        _producerService = producerService;
        _requestTopic = configuration["Kafka:RequestTopic"];
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
    
    private async Task DeployServicesQueueAsync(IEnumerable<Service> servicesQueue, CancellationTokenSource cts)
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
                DockerRequest.CreateContainer,
                JsonSerializer.Serialize(service));

            await _producerService.Produce(_requestTopic, JsonSerializer.Serialize(request));
            await _cache.SetStringAsync(request.Guid.ToString(), "deploy in progress", new DistributedCacheEntryOptions()
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10)
            });

            bool isDeployed = false;
            while (!cts.IsCancellationRequested)
            {
                Console.WriteLine($"Waiting deployment of {service.Id}");
                await Task.Delay(1000);
                var status = await _cache.GetStringAsync(request.Guid.ToString());
                switch (status)
                {
                    case "Ok":
                        isDeployed = true;
                        break;
                    case "Failed":
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
        foreach (var service in servicesQueue) {
            Request<DockerRequest> request = new(
                Guid.NewGuid(),
                DockerRequest.DeleteContainer,
                JsonSerializer.Serialize(service));

                //await _producerService.Produce("docker-requests", JsonSerializer.Serialize(request));
            await _cache.SetStringAsync(request.Guid.ToString(), "rollback in progress");
        }
    }

    private void MakeDockerOptions(Service service)
    {
        var options = service.Options.GroupBy(x => x.Name).ToArray();
        service.Options.Clear();
        foreach (var grouping in options)
        {
            switch (grouping.Key)
            {
                case "image":
                    service.Options.Add(new Option(){Name = "Image", Type = typeof(string).ToString(), Value = JsonSerializer.Serialize(grouping.First().Value)});
                    break;
                case "name":
                    service.Options.Add(new Option(){Name = "Name", Type = typeof(string).ToString(), Value = JsonSerializer.Serialize(grouping.First().Value)});
                    break;
                case "tty":
                    service.Options.Add(new Option(){Name = "Tty", Type = typeof(bool).ToString(), Value = JsonSerializer.Serialize(grouping.First().Value == "true")});
                    break;
                case "ports":
                    HostConfig hostConfig = new HostConfig
                    {
                        PortBindings = new Dictionary<string, IList<PortBinding>>()
                    };

                    IDictionary<string, EmptyStruct> exposedPorts = new Dictionary<string, EmptyStruct>();
                    foreach (var option in grouping)
                    {
                        var mapping = option.Value.Split(':');
                        if (!hostConfig.PortBindings.TryAdd(mapping[1],
                                new List<PortBinding>() {new() {HostPort = mapping[0]}}))
                        {
                            hostConfig.PortBindings[mapping[1]].Add(new PortBinding() {HostPort = mapping[0]});
                        }

                        exposedPorts.TryAdd(mapping[1], new EmptyStruct());
                    }
                    service.Options.Add(new Option() {Name = "HostConfig", Type = hostConfig.GetType().AssemblyQualifiedName, Value = JsonSerializer.Serialize(hostConfig)});
                    service.Options.Add(new Option() {Name = "ExposedPorts", Type = exposedPorts.GetType().AssemblyQualifiedName, Value = JsonSerializer.Serialize(exposedPorts)});
                    break;
            }
        }
        return;
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