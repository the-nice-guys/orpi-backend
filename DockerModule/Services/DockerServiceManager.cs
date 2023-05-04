using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;
using Docker.DotNet;
using Docker.DotNet.Models;
using DockerModule.Interfaces;
using JetBrains.Annotations;
using OrpiLibrary.Models;
using OrpiLibrary.Models.Common;
using OrpiLibrary.Models.Docker.Enums;

namespace DockerModule.Services;

[UsedImplicitly]
public class DockerServiceManager : IServiceManager {
    public async Task CreateService(Service service, Guid requestId, IResponder responder) {
        var client = GetServiceClient(service);
        var parameters = new CreateContainerParameters();
        var message = $"Successfully created container with name {service.Name}.";
        var result = DockerResponse.Ok;

        try {
            SetParameters(parameters, service.Options);
            await client.Containers.CreateContainerAsync(parameters);
        } catch (Exception exception) {
            message = $"Failed to create container with name {service.Name}: {exception.Message}";
            result = DockerResponse.Failed;
        }
        
        await responder.SendResponse(new Response<DockerResponse>(requestId, result, message));
    }

    #region StartService
    
    public async Task StartService(Service service, Guid requestId, IResponder responder) {
        var client = GetServiceClient(service);
        var parameters = new ContainerStartParameters();
        string message;
        DockerResponse result;

        try {
            SetParameters(parameters, service.Options);
            var success = await client.Containers.StartContainerAsync(service.Name, parameters);
            message = success ? $"Successfully started container with name {service.Name}." : 
                $"Failed to start container with name {service.Name}.";
            result = success ? DockerResponse.Ok : DockerResponse.Failed;
        } catch (Exception exception) {
            message = $"Failed to start container with name {service.Name}: {exception.Message}";
            result = DockerResponse.Ok;
        }
        
        await responder.SendResponse(new Response<DockerResponse>(requestId, result, message));
    }
    
    #endregion
    
    #region StopService
    
    public async Task StopService(Service service, Guid requestId, IResponder responder) {
        var client = GetServiceClient(service);
        var parameters = new ContainerStopParameters();
        string message;
        DockerResponse result;
        
        try {
            SetParameters(parameters, service.Options);
            var success = await client.Containers.StopContainerAsync(service.Name, parameters);
            message = success ? $"Successfully stopped container with name {service.Name}." : 
                $"Failed to stop container with name {service.Name}.";
            result = success ? DockerResponse.Ok : DockerResponse.Failed;
        } catch (Exception exception) {
            message = $"Failed to stop container with name {service.Name}: {exception.Message}";
            result = DockerResponse.Ok;
        }
        
        responder.SendResponse(new Response<DockerResponse>(requestId, result, message));
    }
    
    #endregion
    
    #region RestartService
    
    public async Task RestartService(Service service, Guid requestId, IResponder responder) {
        var client = GetServiceClient(service);
        var parameters = new ContainerRestartParameters();
        var message = $"Successfully restarted container with name {service.Name}.";
        var result = DockerResponse.Ok;
        
        try {
            SetParameters(parameters, service.Options);
            await client.Containers.RestartContainerAsync(service.Name, parameters);
        } catch (Exception exception) {
            result = DockerResponse.Failed;
            message = $"Failed to restart container with name {service.Name}: {exception.Message}";
        }
        
        responder.SendResponse(new Response<DockerResponse>(requestId, result, message));
    }
    
    #endregion

    #region DeleteService

    public async Task DeleteService(Service service, Guid requestId, IResponder responder) {
        var client = GetServiceClient(service);
        var parameters = new ContainerRemoveParameters();
        var message = $"Successfully deleted container with name {service.Name}.";
        var result = DockerResponse.Ok;
        
        try {
            SetParameters(parameters, service.Options);
            await client.Containers.RemoveContainerAsync(service.Name, parameters);
        } catch (Exception exception) {
            result = DockerResponse.Failed;
            message = $"Failed to delete container with name {service.Name}: {exception.Message}";
        }
        
        responder.SendResponse(new Response<DockerResponse>(requestId, result, message));
    }
    
    #endregion

    private static DockerClient GetServiceClient(Service service) {
        var uri = new Uri($"http://{service.Ip}");
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