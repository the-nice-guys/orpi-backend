using System;
using System.Threading.Tasks;
using Docker.DotNet;
using Docker.DotNet.Models;
using MonitoringModule.Extensions;
using MonitoringModule.Interfaces;
using OrpiLibrary.Models;

namespace MonitoringModule.Services; 

public class MonitoringService : IMonitoringService {
    public async Task<LoadData?> GetLoadData(string serverEndpoint, string serviceId) {
        var uri = new Uri($"http://{serverEndpoint}");
        var client = new DockerClientConfiguration(uri).CreateClient();
        var loadData = new LoadData();
        var response = new Progress<ContainerStatsResponse>(stats => {
            loadData.CpuUsage = GetCpuUsage(stats);
            loadData.MemoryUsage = GetMemoryUsage(stats);
            loadData.TotalInputNetworkUsage = stats.Networks[Network].RxBytes.ToMegabytes();
            loadData.TotalOutputNetworkUsage = stats.Networks[Network].TxBytes.ToMegabytes();
        });
        
        await client.Containers.GetContainerStatsAsync(serviceId, new ContainerStatsParameters {Stream = false}, response);
        return loadData;
    }

    private const string Network = "eth0";

    private static double GetCpuUsage(ContainerStatsResponse stats) {
        var cpuDelta = stats.CPUStats.CPUUsage.TotalUsage - stats.PreCPUStats.CPUUsage.TotalUsage;
        var systemCpuDelta = stats.CPUStats.SystemUsage - stats.PreCPUStats.SystemUsage;
        return Math.Min(Math.Round(100d * cpuDelta / systemCpuDelta * stats.CPUStats.OnlineCPUs, 2), 100d);
    }

    private static double GetMemoryUsage(ContainerStatsResponse stats) {
        return Math.Min(Math.Round(100d * stats.MemoryStats.Usage / stats.MemoryStats.Limit, 2), 100d);
    }
}