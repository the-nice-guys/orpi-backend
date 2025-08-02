using System.Threading;
using System.Threading.Tasks;
using DockerModule.Services;
using Microsoft.AspNetCore.Mvc;
using OrpiLibrary.Models.Common.Requests;

namespace DockerModule.Controllers;

[ApiController]
[Route("update")]
public class UpdateController: Controller
{
    private readonly IUpdateRepository _updateRepository;
    
    public UpdateController(IUpdateRepository updateRepository)
    {
        _updateRepository = updateRepository;
    }

    [HttpPost("pull")]
    public async Task<IActionResult> Pull(
        [FromBody] PullUpdateRequest request,
        CancellationToken cancellationToken)
    {
        return Ok(_updateRepository.PullUpdates(request.ServerIpAddress, cancellationToken));
    }
}
