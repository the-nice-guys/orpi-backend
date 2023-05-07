using System;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using MonitoringModule.Interfaces;
using MonitoringModule.Models;

namespace MonitoringModule.Hubs; 

public class MonitoringHub : Hub {
    public MonitoringHub(IMonitoringService monitoringService) {
        _monitoringService = monitoringService;
    }
    
    public async Task RequestLoadData(LoadDataRequest request) {
        var data = await _monitoringService.GetLoadData(request.ServerEndpoint, request.ServiceId);
        var timestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
        await Clients.Caller.SendAsync(ReceiverMethodName, new LoadDataResponse()
        {
            ServiceId = request.ServiceId,
            Timestamp = timestamp,
            LoadData = data
        });
    }

    private const string ReceiverMethodName = "ReceiveLoadData";

    private readonly IMonitoringService _monitoringService;
}