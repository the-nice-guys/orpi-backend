using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Confluent.Kafka;

namespace DockerModule.Services {
    public class KafkaConsumerHostedService : IHostedService {
        private const string Topic = "docker-requests";
        private const string GroupId = "test-group";
        private const string BootstrapServers = "localhost:9092";
        public Task StartAsync(CancellationToken cancellationToken) {
            Task.Run(() => StartToConsume(cancellationToken), cancellationToken);
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;

        private static void StartToConsume(CancellationToken cancellationToken) {
            var config = new ConsumerConfig {
                GroupId = GroupId,
                BootstrapServers = BootstrapServers,
            };
            
            using var consumer = new ConsumerBuilder<Ignore, string>(config).Build();
            consumer.Subscribe(Topic);
            while (!cancellationToken.IsCancellationRequested) {
                var message = consumer.Consume(cancellationToken).Message.Value;
            }
        }
    }
}