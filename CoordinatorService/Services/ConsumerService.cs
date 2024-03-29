using System.Text.Json;
using Confluent.Kafka;
using coordinator_service.Models;
using Microsoft.Extensions.Caching.Distributed;
using OrpiLibrary.Models.Common;
using OrpiLibrary.Models.Docker.Enums;

namespace coordinator_service.Services;

public class ConsumerService: IHostedService
{
    private readonly string _topic;
    private readonly string _groupId = "test_group";
    private readonly string _bootstrapServers;
    private readonly IDistributedCache _cache;
    
    public ConsumerService(IDistributedCache cache, string server, string topic)
    {
        _topic = topic;
        _bootstrapServers = server;
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
            Response<DockerResponse> response = JsonSerializer.Deserialize<Response<DockerResponse>>(consumeResult.Message.Value);

            switch (response.Result)
            {
                case DockerResponse.Ok:
                    await _cache.SetStringAsync(response.Guid.ToString(), DockerResponse.Ok.ToString());
                    // await _cache.SetStringAsync(response.Guid.ToString() + "-message", response.Message);
                    break;
                case DockerResponse.Failed:
                    await _cache.SetStringAsync(response.Guid.ToString(), DockerResponse.Failed.ToString());
                    break;
            }
            
            //await _cache.SetStringAsync(response.ServiceId.ToString(), response.Result);
        }
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        Task.Run(() => Consume(_topic, cancellationToken), cancellationToken);
        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}