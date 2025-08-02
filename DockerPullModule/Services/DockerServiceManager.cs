using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;
using Docker.DotNet;
using Docker.DotNet.Models;
using OrpiLibrary.Models;

namespace DockerPullModule.Services;

public class DockerServiceManager : IServiceManager {
    public async Task CreateService(Service service) {
        var client = GetServiceClient();
        var parameters = new CreateContainerParameters();
        SetParameters(parameters, service.Options);
        await client.Containers.CreateContainerAsync(parameters);
    }

    #region StartService
    
    public async Task StartService(Service service) {
        var client = GetServiceClient();
        var parameters = new ContainerStartParameters();
        SetParameters(parameters, service.Options);
        await client.Containers.StartContainerAsync(service.Name, parameters);
    }
    
    #endregion
    
    #region StopService
    
    public async Task StopService(Service service) {
        var client = GetServiceClient();
        var parameters = new ContainerStopParameters();
        SetParameters(parameters, service.Options);
        await client.Containers.StopContainerAsync(service.Name, parameters);
    }
    
    #endregion
    
    #region RestartService
    
    public async Task RestartService(Service service) {
        var client = GetServiceClient();
        var parameters = new ContainerRestartParameters();
        SetParameters(parameters, service.Options);
        await client.Containers.RestartContainerAsync(service.Name, parameters);
    }
    
    #endregion

    #region DeleteService

    public async Task DeleteService(Service service) {
        var client = GetServiceClient();
        var parameters = new ContainerRemoveParameters();
        SetParameters(parameters, service.Options);
        await client.Containers.RemoveContainerAsync(service.Name, parameters);
    }
    
    #endregion

    private static DockerClient GetServiceClient() {
        var uri = new Uri($"http://localhost");
        return new DockerClientConfiguration(uri).CreateClient();
    }

    private static void SetParameters<TParameters>(TParameters parameters, List<Option> options) where TParameters : notnull {
        foreach (var option in options) {
            var property = parameters.GetType().GetProperty(option.Name);
            var optionType = Type.GetType(option.Type);
            if (optionType is null)
                continue;
            
            var deserializedValue = JsonSerializer.Deserialize(option.Value, optionType);
            property?.SetValue(parameters, deserializedValue);
        }
    }
}
