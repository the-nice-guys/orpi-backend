using System;
using System.Reflection;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;
using Docker.DotNet;
using Docker.DotNet.Models;
using DockerModule.Interfaces;
using JetBrains.Annotations;
using OrpiLibrary.Models;
using OrpiLibrary.Models.Docker.Enums;

namespace DockerModule.Services {
    [UsedImplicitly]
    public class DockerServiceManager : IServiceManager {
        public async Task CreateService(Service service, IResponder responder) {
            var client = GetServiceClient(service);
            var parameters = new CreateContainerParameters();
            SetParameters(parameters, service.Options);
            var response = await client.Containers.CreateContainerAsync(parameters);
            responder.SendResponse(DockerResponse.Ok, $"Container with id {response.ID} created");
        }

        #region StartService
        
        public async Task StartService(Service service, IResponder responder) {
            var client = GetServiceClient(service);
            var parameters = new ContainerStartParameters();
            SetParameters(parameters, service.Options);
            var success = await client.Containers.StartContainerAsync(service.Name, parameters);
            if (success)
                responder.SendResponse(DockerResponse.Ok, $"Successfully started container with name {service.Name}");
            else 
                responder.SendResponse(DockerResponse.Failed, $"Failed to start container with name {service.Name}");
        }
        
        #endregion
        
        #region StopService
        
        public async Task StopService(Service service, IResponder responder) {
            var client = GetServiceClient(service);
            var parameters = new ContainerStopParameters();
            SetParameters(parameters, service.Options);
            var success = await client.Containers.StopContainerAsync(service.Name, parameters);
            if (success)
                responder.SendResponse(DockerResponse.Ok, $"Successfully stopped container with name {service.Name}");
            else 
                responder.SendResponse(DockerResponse.Failed, $"Failed to stop container with name {service.Name}");
        }
        
        #endregion
        
        #region RestartService
        
        public async Task RestartService(Service service, IResponder responder) {
            var client = GetServiceClient(service);
            var parameters = new ContainerRestartParameters();
            SetParameters(parameters, service.Options);
            try {
                await client.Containers.RestartContainerAsync(service.Name, parameters);
            } catch {
                responder.SendResponse(DockerResponse.Failed, $"Failed to stop container with name {service.Name}");
            }
            
            responder.SendResponse(DockerResponse.Ok, $"Successfully stopped container with name {service.Name}");
        }
        
        #endregion

        #region DeleteService

        public async Task DeleteService(Service service, IResponder responder) {
            var client = GetServiceClient(service);
            var parameters = new ContainerRemoveParameters();
            SetParameters(parameters, service.Options);
            try {
                await client.Containers.RemoveContainerAsync(service.Name, parameters);
            } catch {
                responder.SendResponse(DockerResponse.Failed, $"Failed to remove container with name {service.Name}");
            }
            
            responder.SendResponse(DockerResponse.Ok, $"Successfully stopped container with name {service.Name}");
        }
        
        #endregion

        private static DockerClient GetServiceClient(Service service) {
            var uri = new Uri($"http://{service.Ip}:{DefaultDockerApiPort}");
            return new DockerClientConfiguration(uri).CreateClient();
        }

        private static void SetParameters<TParameters>(TParameters parameters, List<Option> options) where TParameters : notnull {
            foreach (var option in options) {
                var property = parameters.GetType().GetProperty(option.Name);
                SetParameter(property, parameters, option);
            }
        }

        #region SetParameterValue

        // TODO: fix
        private static void SetParameter<TParameters>(PropertyInfo? property, TParameters parameters, Option option) {
            if (option.Type == typeof(bool))
                DeserializeAndSetValue<bool, TParameters>(property, parameters, option.Value);
            else if (option.Type == typeof(string))
                DeserializeAndSetValue<string, TParameters>(property, parameters, option.Value);
            else if (option.Type == typeof(TimeSpan?))
                DeserializeAndSetValue<TimeSpan?, TParameters>(property, parameters, option.Value);
            else if (option.Type == typeof(HostConfig))
                DeserializeAndSetValue<HostConfig, TParameters>(property, parameters, option.Value);
            else if (option.Type == typeof(HealthConfig))
                DeserializeAndSetValue<HealthConfig, TParameters>(property, parameters, option.Value);
            else if (option.Type == typeof(NetworkingConfig))
                DeserializeAndSetValue<NetworkingConfig, TParameters>(property, parameters, option.Value);
            else if (option.Type == typeof(IList<string>))
                DeserializeAndSetValue<IList<string>, TParameters>(property, parameters, option.Value);
            else if (option.Type == typeof(IDictionary<string, string>)) 
                DeserializeAndSetValue<IDictionary<string, string>, TParameters>(property, parameters, option.Value);
            else if (option.Type == typeof(IDictionary<string, EmptyStruct>)) 
                DeserializeAndSetValue<IDictionary<string, EmptyStruct>, TParameters>(property, parameters, option.Value);
        }
        
        #endregion

        private static void DeserializeAndSetValue<TOption, TParameters>(PropertyInfo? property, TParameters parameters, string value) {
            var deserializedValue = JsonSerializer.Deserialize<TOption>(value);
            property?.SetValue(parameters, deserializedValue);
        }

        private const uint DefaultDockerApiPort = 2375;
    }
}