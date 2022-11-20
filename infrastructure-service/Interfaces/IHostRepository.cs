namespace infrastructure_service.Interfaces;

public interface IHostRepository
{
    public Task<Models.Host> Get(long id);
    public Task<bool> InsertInfrastructureHost(long infrastructureId, long hostId);
    public Task<IEnumerable<Models.Host>> GetAllForInfrastructure(long id);
    public Task<long> Create(Models.Host host);
    public Task<bool> Update(Models.Host host);
    public Task<bool> Delete(long id);
}