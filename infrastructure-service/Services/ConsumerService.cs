using Confluent.Kafka;
using infrastructure_service.Interfaces;

namespace infrastructure_service.Services;

public class ConsumerService: IHostedService
{
    private readonly string _topic = "massages";
    private readonly string _groupId = "test_group";
    private readonly string _bootstrapServers = "localhost:9092";
    private readonly IInfrastructureRepository _infrastructureRepository;
    
    public ConsumerService(IInfrastructureRepository infrastructureRepository)
    {
        _infrastructureRepository = infrastructureRepository;
    }

    async void Consume(CancellationToken cancellationToken)
    {
        var config = new ConsumerConfig
        {
            GroupId = _groupId,
            BootstrapServers = _bootstrapServers,
            AutoOffsetReset = AutoOffsetReset.Earliest
        };
        
        using var consumer = new ConsumerBuilder<Ignore, string>(config).Build();
        consumer.Subscribe(_topic);
        
        while (!cancellationToken.IsCancellationRequested)
        {
            var consumeResult = consumer.Consume(cancellationToken);
            Console.WriteLine($"Consumed message '{consumeResult.Message.Value}' at: '{consumeResult.TopicPartitionOffset}'.");
        }
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        Task.Run(() => Consume(cancellationToken), cancellationToken);
        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}