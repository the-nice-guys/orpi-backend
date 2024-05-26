using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Microsoft.Extensions.Hosting;
using Confluent.Kafka;
using DockerModule.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OrpiLibrary.Models;
using OrpiLibrary.Models.Common;
using OrpiLibrary.Models.Docker.Enums;
using Unity;
using Config = OrpiLibrary.Config;

namespace DockerModule.Services;

public class KafkaConsumerHostedService : IHostedService
{
    private readonly IResponder Responder;
    private readonly IServiceManager ServiceManager;
    private readonly IConfiguration _configuration;
    private readonly IServiceScopeFactory _scopeFactory;

    public KafkaConsumerHostedService(IConfiguration configuration, IServiceScopeFactory factory)
    {
        _scopeFactory = factory;
        var scope = factory.CreateScope();
        _configuration = configuration;
        ServiceManager = scope.ServiceProvider.GetRequiredService<IServiceManager>();
        Responder = scope.ServiceProvider.GetRequiredService<IResponder>();
    }


    public Task StartAsync(CancellationToken cancellationToken) {
        Task.Run(() => StartToConsume(cancellationToken), cancellationToken);
        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;

    private async void StartToConsume(CancellationToken cancellationToken) {
        var config = new ConsumerConfig {
            GroupId = Config.KafkaRequestGroupId,
            BootstrapServers = _configuration["Kafka:BootstrapServers"],
        };
        
        using var consumer = new ConsumerBuilder<Ignore, string>(config).Build();
        consumer.Subscribe(_configuration["Kafka:RequestTopic"]);
        while (!cancellationToken.IsCancellationRequested) {
            var message = consumer.Consume(cancellationToken).Message.Value;
            var request = JsonSerializer.Deserialize<Request<DockerRequest>>(message);
            if (request is null) 
                continue;
            
            var service = JsonSerializer.Deserialize<Service>(request.Payload);
            if (service is null) {
                Responder.SendResponse(new Response<DockerResponse>(
                    guid: request.Guid,
                    result: DockerResponse.Failed,
                    message: "Failed to get service parameters.")
                );
                
                continue;
            }

            var task = request.Type switch {
                DockerRequest.CreateContainer => ServiceManager.CreateService(service, request.Guid, Responder),
                DockerRequest.StartContainer => ServiceManager.StartService(service, request.Guid, Responder),
                DockerRequest.StopContainer => ServiceManager.StopService(service, request.Guid, Responder),
                DockerRequest.RestartContainer => ServiceManager.RestartService(service, request.Guid, Responder),
                DockerRequest.DeleteContainer => ServiceManager.DeleteService(service, request.Guid, Responder),

                _ => new Task(() => Responder.SendResponse(new Response<DockerResponse>(
                    guid: request.Guid,
                    result: DockerResponse.NotFound,
                    message: "Unknown request type.")))
            };
            
            task.Start();
        }
    }
}