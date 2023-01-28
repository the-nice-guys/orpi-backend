using Confluent.Kafka;

namespace coordinator_service.Interfaces;

public interface IProducerService
{
    public Task<DeliveryResult<Null, string>> Produce(string topic, string message);
}