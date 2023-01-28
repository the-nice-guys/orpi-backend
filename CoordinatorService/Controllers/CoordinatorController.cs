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
    
    public CoordinatorController(IDeploymentService deploymentService)
    {
        _deploymentService = deploymentService;
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
}