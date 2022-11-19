using infrastructure_service.Interfaces;
using infrastructure_service.Models;
using Microsoft.AspNetCore.Mvc;

namespace infrastructure_service.Controllers;
[ApiController]
[Route("[controller]")]
public class ServiceController : Controller
{
    private ILogger<ServiceController> _logger;
    private IServiceRepository _serviceRepository;
    
    public ServiceController(ILogger<ServiceController> logger, IServiceRepository serviceRepository)
    {
        _logger = logger;
        _serviceRepository = serviceRepository;
    }
    
    [HttpGet]
    [Route("get_service")]
    public async Task<IActionResult> GetService(long id)
    {
        var service = await _serviceRepository.Get(id);
        return Ok(service);
    }
    
    [HttpPost]
    [Route("create_service")]
    public async Task<IActionResult> CreateService(Service service)
    {
        return Ok(await _serviceRepository.Create(service));
    }
    
    [HttpPut]
    [Route("update_service")]
    public async Task<IActionResult> UpdateService(Service service)
    {
        await _serviceRepository.Update(service);
        return Ok();
    }
    
    [HttpDelete]
    [Route("delete_service")]
    public async Task<IActionResult> DeleteService(long id)
    {
        await _serviceRepository.Delete(id);
        return Ok();
    }

}