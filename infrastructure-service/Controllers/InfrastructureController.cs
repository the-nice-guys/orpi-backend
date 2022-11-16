using Microsoft.AspNetCore.Mvc;

namespace infrastructure_service.Controllers;

[ApiController]
[Route("[controller]")]
public class InfrastructureController: ControllerBase
{
    private readonly ILogger<InfrastructureController> _logger;

    public InfrastructureController(ILogger<InfrastructureController> logger)
    {
        _logger = logger;
    }
}