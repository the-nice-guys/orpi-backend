using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;

namespace DockerModule.Services {
    public class KafkaConsumerHostedService : IHostedService {
        public Task StartAsync(CancellationToken cancellationToken) {
            Task.Run(() => StartToConsume(cancellationToken), cancellationToken);
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;

        private void StartToConsume(CancellationToken cancellationToken) {
            while (!cancellationToken.IsCancellationRequested) {
                
            }
        }
    }
}