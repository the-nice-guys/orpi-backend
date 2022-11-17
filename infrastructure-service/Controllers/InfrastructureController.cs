using infrastructure_service.Interfaces;
using infrastructure_service.Models;
using Microsoft.AspNetCore.Mvc;

namespace infrastructure_service.Controllers;

[ApiController]
[Route("[controller]")]
public class InfrastructureController: ControllerBase
{
    private readonly ILogger<InfrastructureController> _logger;
    private readonly IInfrastructureRepository _repository;

    public InfrastructureController(ILogger<InfrastructureController> logger, IInfrastructureRepository repository)
    {
        _logger = logger;
        _repository = repository;
    }
    
    [HttpGet]
    [Route("get_infrastructure")]
    public async Task<Infrastructure> Get(long id)
    {
        return await _repository.Get(id);
    }
    
    [HttpPost]
    [Route("create_infrastructure")]
    public async Task<long> Create(Infrastructure infrastructure)
    {
        return await _repository.Create(infrastructure);
    }
    
    [HttpPut]
    [Route("update_infrastructure")]
    public async Task<bool> Update(Infrastructure infrastructure)
    {
        return await _repository.Update(infrastructure);
    }
    
    [HttpDelete]
    [Route("delete_infrastructure")]
    public async Task<bool> Delete(long id)
    {
        return await _repository.Delete(id);
    }
}