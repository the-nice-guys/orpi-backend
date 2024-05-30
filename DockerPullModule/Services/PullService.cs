using System.Linq;
using System.Threading.Tasks;
using OrpiLibrary.Models.Docker.Enums;

namespace DockerPullModule.Services;

public class PullService
{
    private readonly IServiceManager _serviceManager;
    private readonly IDockerModuleClient _dockerModuleClient;
    public PullService(
        IServiceManager serviceManager,
        IDockerModuleClient dockerModuleClient)
    {
        _serviceManager = serviceManager;
        _dockerModuleClient = dockerModuleClient;
    }
    
    public async Task Run()
    {
        while (true)
        {
            foreach (var update in (await _dockerModuleClient.GetUpdates()).OrderBy(x => x.CreatedOn))
            {
                await (update.UpdateType switch {
                    DockerRequest.CreateContainer => _serviceManager.CreateService(update.Service),
                    DockerRequest.StartContainer => _serviceManager.StartService(update.Service),
                    DockerRequest.StopContainer => _serviceManager.StopService(update.Service),
                    DockerRequest.RestartContainer => _serviceManager.RestartService(update.Service),
                    DockerRequest.DeleteContainer => _serviceManager.DeleteService(update.Service),
                    _ => Task.CompletedTask
                });
            }
        }
    }
}
