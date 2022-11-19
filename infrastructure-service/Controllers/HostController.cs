using infrastructure_service.Interfaces;
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
    public async Task<Models.Host> Get(long id)
    {
        return await _hostRepository.Get(id);
    }
    
    [HttpPost]
    [Route("create_host")]
    public async Task<long> Create(Models.Host host)
    {
        return await _hostRepository.Create(host);
    }
    
    [HttpPut]
    [Route("update_host")]
    public async Task<bool> Update(Models.Host host)
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