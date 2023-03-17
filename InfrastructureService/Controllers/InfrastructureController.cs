using infrastructure_service.Interfaces;
using Microsoft.AspNetCore.Authorization;
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
    private readonly IOptionsRepository _optionsRepository;

    public InfrastructureController(
        ILogger<InfrastructureController> logger,
        IInfrastructureRepository infrastructureRepository,
        IHostRepository hostRepository,
        IServiceRepository serviceRepository,
        IOptionsRepository optionsRepository)
    {
        _logger = logger;
        _infrastructureRepository = infrastructureRepository;
        _hostRepository = hostRepository;
        _serviceRepository = serviceRepository;
        _optionsRepository = optionsRepository;
    }
    
    [HttpGet]
    [Authorize]
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
        
        foreach (var host in request.Infrastructure.Hosts)
        {
            hosts.Add(host);
            var hostId = await _hostRepository.Create(host);
            host.Id = hostId;

            foreach (var hostService in host.Services)
            {
                var serviceId = await _serviceRepository.Create(hostService);
                hostService.Id = serviceId;
                
                foreach (var option in hostService.Options)
                {
                    var optionId = await _optionsRepository.Create(option);
                    option.Id = optionId;
                }
            }
        }
        
        request.Infrastructure.Hosts = hosts;
        var infraId =  await _infrastructureRepository.Create(request.Infrastructure);
        request.Infrastructure.Id = infraId;
        
        await _infrastructureRepository.InsertUserInfrastructure(request.UserId, infraId);

        foreach (var host in hosts)
        {
            await _hostRepository.InsertInfrastructureHost(infraId, host.Id);

            foreach (var hostService in host.Services)
            {
                await _serviceRepository.InsertHostService(host.Id, hostService.Id);
                
                foreach (var option in hostService.Options)
                {
                    await _optionsRepository.InsertServiceOption(hostService.Id, option.Id);
                }
            }
        }
        
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