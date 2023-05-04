using System;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using MonitoringModule.Interfaces;

namespace MonitoringModule.Hubs; 

public class MonitoringHub : Hub {
    public MonitoringHub(IMonitoringService monitoringService) {
        _monitoringService = monitoringService;
    }
    
    public async Task RequestLoadData(string serverEndpoint, string serviceId) {
        var data = await _monitoringService.GetLoadData(serverEndpoint, serviceId);
        var timestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
        await Clients.Caller.SendAsync(ReceiverMethodName, serviceId, timestamp, JsonSerializer.Serialize(data));
    }

    private const string ReceiverMethodName = "ReceiveLoadData";

    private readonly IMonitoringService _monitoringService;
}