using infrastructure_service.Interfaces;
using OrpiLibrary.Models;
using Host = OrpiLibrary.Models.Host;
using Microsoft.AspNetCore.Mvc;

namespace infrastructure_service.Controllers;

[ApiController]
[Route("[controller]")]
public class HostController : Controller
{
    private ILogger<HostController> _logger;
    private IHostRepository _hostRepository;
    
    public HostController(ILogger<HostController> logger, IHostRepository hostRepository)
    {
        _logger = logger;
        _hostRepository = hostRepository;
    }
    
    [HttpGet]
    [Route("get_host")]
    public async Task<Host> Get(long id)
    {
        return await _hostRepository.Get(id);
    }
    
    [HttpGet]
    [Route("get_all_for_infrastructure")]
    public async Task<IEnumerable<Host>> GetForInfrastructure(long id)
    {
        return await _hostRepository.GetAllForInfrastructure(id);
    }
    
    [HttpPost]
    [Route("create_host")]
    public async Task<long> Create(Host host)
    {
        return await _hostRepository.Create(host);
    }
    
    [HttpPost]
    [Route("add_host_to_infrastructure")]
    public async Task<long> AddToInfrastructure(AddHostRequest request)
    {
        var id = await _hostRepository.Create(request.Host);
        await _hostRepository.InsertInfrastructureHost(request.InfrastructureId, id);
        return id;
    }
    
    [HttpPut]
    [Route("update_host")]
    public async Task<bool> Update(Host host)
    {
        return await _hostRepository.Update(host);
    }
    
    [HttpDelete]
    [Route("delete_host")]
    public async Task<bool> Delete(long id)
    {
        return await _hostRepository.Delete(id);
    }
}