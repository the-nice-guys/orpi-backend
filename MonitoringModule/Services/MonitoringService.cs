using System.Threading.Tasks;
using MonitoringModule.Interfaces;
using OrpiLibrary.Models;

namespace MonitoringModule.Services; 

public class MonitoringService : IMonitoringService {
    public async Task<LoadData> GetLoadData(string serviceId) => new LoadData();
}