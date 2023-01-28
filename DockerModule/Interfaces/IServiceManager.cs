using System.Threading.Tasks;
using OrpiLibrary.Models;

namespace DockerModule.Interfaces {
    public interface IServiceManager {
        public Task CreateService(Service service, IResponder responder);
        public Task StartService(Service service, IResponder responder);
        public Task StopService(Service service, IResponder responder);
        public Task RestartService(Service service, IResponder responder);
        public Task DeleteService(Service service, IResponder responder);
    }
}