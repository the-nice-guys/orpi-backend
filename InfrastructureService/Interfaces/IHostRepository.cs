using OrpiLibrary.Models;
using Host = OrpiLibrary.Models.Host;

namespace infrastructure_service.Interfaces;

public interface IHostRepository
{
    public Task<Host> Get(long id);
    public Task<bool> InsertInfrastructureHost(long infrastructureId, long hostId);
    public Task<IEnumerable<Host>> GetAllForInfrastructure(long id);
    public Task<long> Create(Host host);
    public Task<bool> Update(Host host);
    public Task<bool> Delete(long id);
}