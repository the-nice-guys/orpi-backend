using System.Threading.Tasks;
using MonitoringModule.Interfaces;
using OrpiLibrary.Models;

namespace MonitoringModule.Services; 

public class MonitoringService : IMonitoringService {
    public Task<LoadData> GetLoadData(string serviceId) {
        var task = new Task<LoadData>(() => new LoadData());
        task.Start();
        return task;
    }
}