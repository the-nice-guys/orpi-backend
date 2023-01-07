using infrastructure_service.Interfaces;
using OrpiLibrary.Models;
using Microsoft.AspNetCore.Mvc;
using Host = OrpiLibrary.Models.Host;

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
    public async Task<long> CreateInfrastructure(CreateInfrastructureRequest request)
    {
        List<Host> hosts = new List<Host>();
        request.Infrastructure.Hosts?.ForEach(async host =>
        {
            hosts.Add(host);
            var hostId = await _hostRepository.Create(host);
            host.Id = hostId;

            foreach (var hostService in host.Services)
            {
                var serviceId = await _serviceRepository.Create(hostService);
                hostService.Id = serviceId;
            }
        });
        
        request.Infrastructure.Hosts = hosts;
        var infraId =  await _infrastructureRepository.Create(request.Infrastructure);
        request.Infrastructure.Id = infraId;
        
        await _infrastructureRepository.InsertUserInfrastructure(request.UserId, infraId);
        hosts.ForEach(async host =>
        {
            await _hostRepository.InsertInfrastructureHost(infraId, host.Id);

            foreach (var hostService in host.Services)
            {
                await _serviceRepository.InsertHostService(host.Id, hostService.Id);
            }
        });
        return infraId;
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