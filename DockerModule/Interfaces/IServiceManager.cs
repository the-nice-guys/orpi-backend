using System;
using System.Threading.Tasks;
using OrpiLibrary.Models;

namespace DockerModule.Interfaces;

public interface IServiceManager {
    public Task CreateNetwork(Network network, Guid requestId, IResponder responder);
    public Task CreateService(Service service, Guid requestId, IResponder responder);
    public Task StartService(Service service, Guid requestId, IResponder responder);
    public Task StopService(Service service, Guid requestId, IResponder responder);
    public Task RestartService(Service service, Guid requestId, IResponder responder);
    public Task DeleteService(Service service, Guid requestId, IResponder responder);
}