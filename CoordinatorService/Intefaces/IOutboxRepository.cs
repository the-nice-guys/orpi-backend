using coordinator_service.Models;

namespace coordinator_service.Intefaces;

public interface IOutboxRepository
{
    public Task AddOutboxMessage(OutboxMessage message);
    public Task<OutboxMessage> GetUnlockedMassage();
    public Task DeleteMessage(OutboxMessage message);
}