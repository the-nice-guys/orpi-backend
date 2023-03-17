using OrpiLibrary.Models;

namespace infrastructure_service.Interfaces;

public interface IOptionsRepository
{
    public Task<Option> Get(long id);
    public Task<bool> InsertServiceOption(long serviceId, long optionId);
    public Task<IEnumerable<Option>> GetAllForService(long serviceId);
    public Task<bool> Update(Option options);
    public Task<bool> Delete(long id);
    public Task<long> Create(Option option);
}