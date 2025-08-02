using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Confluent.Kafka;
using DockerModule.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OrpiLibrary.Models;
using OrpiLibrary.Models.Common;
using OrpiLibrary.Models.Docker.Enums;
using Config = OrpiLibrary.Config;

namespace DockerModule.Services;

public class KafkaConsumerHostedService : IHostedService
{
    private readonly IResponder _responder;
    private readonly IConfiguration _configuration;
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly IUpdateRepository _updateRepository;

    public KafkaConsumerHostedService(IConfiguration configuration, IServiceScopeFactory factory)
    {
        _scopeFactory = factory;
        var scope = factory.CreateScope();
        _configuration = configuration;
        _responder = scope.ServiceProvider.GetRequiredService<IResponder>();
        _updateRepository = scope.ServiceProvider.GetRequiredService<IUpdateRepository>();
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
                _responder.SendResponse(new Response<DockerResponse>(
                    guid: request.Guid,
                    result: DockerResponse.Failed,
                    message: "Failed to get service parameters.")
                );
                
                continue;
            }

            await _updateRepository.PushUpdate(request.Guid, service.Ip, request.Type, request.Payload, cancellationToken);
        }
    }
}
