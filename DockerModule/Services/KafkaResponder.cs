using System;
using System.Text.Json;
using System.Threading.Tasks;
using Confluent.Kafka;
using DockerModule.Interfaces;
using JetBrains.Annotations;
using OrpiLibrary.Models.Common;
using OrpiLibrary.Models.Docker.Enums;
using Config = OrpiLibrary.Config;

namespace DockerModule.Services;

[UsedImplicitly]
public class KafkaResponder : IResponder {
    public KafkaResponder() {
        var config = new ConsumerConfig {
            BootstrapServers = $"{Config.KafkaBootstrapServerHost}:{Config.KafkaBootstrapServerPort}"
        };
        
        _producer = new ProducerBuilder<Null, string>(config).Build();
    }
    
    public async Task SendResponse(Response<DockerResponse> response) { 
        Console.ForegroundColor = response.Result switch {
            DockerResponse.Ok => ConsoleColor.Green,
            DockerResponse.Failed => ConsoleColor.Red,
            DockerResponse.NotFound => ConsoleColor.Yellow,
            
            _ => ConsoleColor.Black
        }; 
        
        Console.WriteLine($"{response.Result}: {response.Message}");
        Console.ForegroundColor = ConsoleColor.Black;

        var message = JsonSerializer.Serialize(response);

        await _producer.ProduceAsync(Config.KafkaResponseTopic, new Message<Null, string> {Value = message});
    }

    private readonly IProducer<Null, string> _producer;
}