using System.Text.Json;
using Confluent.Kafka;
using coordinator_service.Models;
using Microsoft.Extensions.Caching.Distributed;

namespace coordinator_service.Services;

public class ConsumerService: IHostedService
{
    private readonly string[] _topics;
    private readonly string _groupId = "test_group";
    private readonly string _bootstrapServers = "localhost:9092";
    private readonly IDistributedCache _cache;
    
    public ConsumerService(IDistributedCache cache, params string[] topics)
    {
        _topics = topics;
        _cache = cache;
    }

    async void Consume(string topic, CancellationToken cancellationToken)
    {
        var config = new ConsumerConfig
        {
            GroupId = _groupId,
            BootstrapServers = _bootstrapServers,
            //AutoOffsetReset = AutoOffsetReset.Earliest
        };
        
        using var consumer = new ConsumerBuilder<Ignore, string>(config).Build();
        consumer.Subscribe(topic);
        
        while (!cancellationToken.IsCancellationRequested)
        {
            var consumeResult = consumer.Consume(cancellationToken);
            Console.WriteLine($"Consumed message from '{topic}' '{consumeResult.Message.Value}' at: '{consumeResult.TopicPartitionOffset}'.");
            DeploymentResponse response = JsonSerializer.Deserialize<DeploymentResponse>(consumeResult.Message.Value);
            await _cache.SetStringAsync(response.ServiceId.ToString(), response.Result);
        }
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        foreach (var topic in _topics)
        {
            Task.Run(() => Consume(topic, cancellationToken), cancellationToken);
        }

        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}