using System.Threading.Tasks;
using OrpiLibrary.Models;

namespace MonitoringModule.Interfaces; 

public interface IMonitoringService {
    public Task<LoadData?> GetLoadData(string serverEndpoint, string serviceId);
}