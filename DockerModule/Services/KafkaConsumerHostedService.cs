using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Microsoft.Extensions.Hosting;
using Confluent.Kafka;
using DockerModule.Interfaces;
using OrpiLibrary.Models;
using OrpiLibrary.Models.Common;
using OrpiLibrary.Models.Docker.Enums;
using Unity;

namespace DockerModule.Services {
    public class KafkaConsumerHostedService : IHostedService {
        [Dependency] [UsedImplicitly] public IResponder Responder { get; set; } = null!;
        [Dependency] [UsedImplicitly] public IServiceManager ServiceManager { get; set; } = null!;
        
        public Task StartAsync(CancellationToken cancellationToken) {
            Task.Run(() => StartToConsume(cancellationToken), cancellationToken);
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;

        private async void StartToConsume(CancellationToken cancellationToken) {
            var config = new ConsumerConfig {
                GroupId = GroupId,
                BootstrapServers = BootstrapServers,
            };
            
            using var consumer = new ConsumerBuilder<Ignore, string>(config).Build();
            consumer.Subscribe(Topic);
            while (!cancellationToken.IsCancellationRequested) {
                var message = consumer.Consume(cancellationToken).Message.Value;
                var request = JsonSerializer.Deserialize<Request<DockerRequestType>>(message);
                if (request is null) {
                    Responder.SendResponse(DockerResponseType.Failed, "Failed to parse request.");
                    continue;
                }
                
                var service = JsonSerializer.Deserialize<Service>(request.Payload);
                if (service is null) {
                    Responder.SendResponse(DockerResponseType.Failed, "Failed to get service parameters.");
                    continue;
                }
                
                await (request.Type switch {
                    DockerRequestType.Create => ServiceManager.CreateService(service, Responder),
                    DockerRequestType.Start => ServiceManager.StartService(service, Responder),
                    DockerRequestType.Stop => ServiceManager.StopService(service, Responder),
                    DockerRequestType.Restart => ServiceManager.RestartService(service, Responder),
                    DockerRequestType.Delete => ServiceManager.DeleteService(service, Responder),

                    _ => Task.CompletedTask
                });
            }
        }
        
        private const string Topic = "docker-requests";
        private const string GroupId = "test-group";
        private const string BootstrapServers = "localhost:9092";
    }
}