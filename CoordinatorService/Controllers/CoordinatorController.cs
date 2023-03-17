using coordinator_service.Interfaces;
using coordinator_service.Models;
using coordinator_service.Utils;
using Microsoft.AspNetCore.Mvc;
using OrpiLibrary.Models;

namespace coordinator_service.Controllers;

[ApiController]
[Route("[controller]")]
public class CoordinatorController : Controller
{
    private IDeploymentService _deploymentService;
    private IStartService _startService;
    
    public CoordinatorController(IDeploymentService deploymentService, IStartService startService)
    {
        _deploymentService = deploymentService;
        _startService = startService;
    }
    
    [HttpPost]
    [Route("deploy-infrastructure")]
    public async Task<IActionResult> DeployInfrastructure([FromBody] InfrastructureDeploymentRequest request)
    {
        var queues = QueueUtil.GetDeploymentQueues(request.Infrastructure);
        
        CancellationTokenSource cancellationTokenSource = new();
        var result = await _deploymentService.DeployServicesQueuesAsync(queues, cancellationTokenSource.Token);
        
        return Ok(result);
    }

    [HttpPost]
    [Route("start-infrastructure")]
    public async Task<IActionResult> StartInfrastructure([FromBody] StartInfrastructureRequest request)
    {
        var queues = QueueUtil.GetDeploymentQueues(request.Infrastructure);
        
        CancellationTokenSource cancellationTokenSource = new();
        var result = await _startService.StartServicesQueuesAsync(queues, cancellationTokenSource.Token);

        return Ok(result);
    }
}