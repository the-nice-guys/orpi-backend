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
    
    [HttpGet]
    [Route("get_services_for_infrastructure")]
    public async Task<IActionResult> GetServicesForInfrastructure(long id)
    {
        var services = await _serviceRepository.GetAllForInfrastructure(id);
        return Ok(services);
    }
    
    [HttpPost]
    [Route("create_service")]
    public async Task<IActionResult> CreateService(Service service)
    {
        return Ok(await _serviceRepository.Create(service));
    }
    
    [HttpPost]
    [Route("add_service_to_host")]
    public async Task<IActionResult> AddToHost(AddServiceRequest request)
    {
        var id = await _serviceRepository.Create(request.Service);
        return Ok(await _serviceRepository.InsertHostService(request.HostId, id));
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