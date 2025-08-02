using System.Threading.Tasks;
using OrpiLibrary.Models;

namespace DockerPullModule.Services;

public interface IServiceManager {
    public Task CreateService(Service service);
    public Task StartService(Service service);
    public Task StopService(Service service);
    public Task RestartService(Service service);
    public Task DeleteService(Service service);
}
