using System.Threading.Tasks;
using DockerPullModule.Services;

namespace DockerPullModule;

public static class Program
{
    public static async Task Main(string[] args)
    {
        var dockerServiceManager = new DockerServiceManager();
        var dockerModuleClient = new DockerModuleClient(args[0]);

        await new PullService(dockerServiceManager, dockerModuleClient).Run();
    }
}
