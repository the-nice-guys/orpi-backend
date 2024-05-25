using Confluent.Kafka;
using coordinator_service.Intefaces;

namespace coordinator_service.Services;

public class OutboxBackgroundService: BackgroundService
{
    private readonly IServiceProvider _serviceProvider;

    public OutboxBackgroundService(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await StartWork(stoppingToken);
    }

    private async Task StartWork(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            var scope = _serviceProvider.CreateScope();

            var outboxRepository = scope.ServiceProvider.GetRequiredService<IOutboxRepository>();
            var producer = scope.ServiceProvider.GetRequiredService<IProducer<Null, string>>();

            var message = await outboxRepository.GetUnlockedMassage();

            await producer.ProduceAsync(message.Topic, new Message<Null, string>()
            {
                Value = message.Data
            }, stoppingToken);

            await outboxRepository.DeleteMessage(message);
        }
    }
}