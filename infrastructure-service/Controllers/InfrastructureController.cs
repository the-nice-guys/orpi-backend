using infrastructure_service.Interfaces;
using infrastructure_service.Models;
using Microsoft.AspNetCore.Mvc;
using Host = infrastructure_service.Models.Host;

namespace infrastructure_service.Controllers;

[ApiController]
[Route("[controller]")]
public class InfrastructureController: ControllerBase
{
    private readonly ILogger<InfrastructureController> _logger;
    private readonly IInfrastructureRepository _infrastructureRepository;
    private readonly IHostRepository _hostRepository;
    private readonly IServiceRepository _serviceRepository;

    public InfrastructureController(
        ILogger<InfrastructureController> logger,
        IInfrastructureRepository infrastructureRepository,
        IHostRepository hostRepository,
        IServiceRepository serviceRepository)
    {
        _logger = logger;
        _infrastructureRepository = infrastructureRepository;
        _hostRepository = hostRepository;
        _serviceRepository = serviceRepository;
    }
    
    [HttpGet]
    [Route("get_infrastructure")]
    public async Task<Infrastructure> GetInfrastructure(long id)
    {
        return await _infrastructureRepository.Get(id);
    }
    
    [HttpGet]
    [Route("get_infrastructures_for_user")]
    public async Task<IEnumerable<Infrastructure>> GetInfrastructuresForUser(long id)
    {
        return await _infrastructureRepository.GetAllForUser(id);
    }
    
    [HttpPost]
    [Route("create_infrastructure")]
    public async Task<long> CreateInfrastructure(Infrastructure infrastructure)
    {
        return await _infrastructureRepository.Create(infrastructure);
    }
    
    [HttpPut]
    [Route("update_infrastructure")]
    public async Task<bool> UpdateInfrastructure(Infrastructure infrastructure)
    {
        return await _infrastructureRepository.Update(infrastructure);
    }
    
    [HttpDelete]
    [Route("delete_infrastructure")]
    public async Task<bool> DeleteInfrastructure(long id)
    {
        return await _infrastructureRepository.Delete(id);
    }
}