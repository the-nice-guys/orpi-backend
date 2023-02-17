using Confluent.Kafka;
using coordinator_service.Interfaces;

namespace coordinator_service.Services;

public class ProducerService: IProducerService
{
    private IProducer<Null, string> _producer;

    public ProducerService(string bootstrapServers)
    {
        var config = new ProducerConfig
        {
            BootstrapServers = bootstrapServers
        };

        _producer = new ProducerBuilder<Null, string>(config).Build();
    }
    
    public async Task<DeliveryResult<Null, string>> Produce(string topic, string message)
    {
        return await _producer.ProduceAsync(topic, new Message<Null, string> { Value = message });
    }
}