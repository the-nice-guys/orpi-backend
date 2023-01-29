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
using OrpiLibrary.Models.Common;
using OrpiLibrary.Models.Docker.Enums;

namespace DockerModule.Services {
    [UsedImplicitly]
    public class DockerServiceManager : IServiceManager {
        public async Task CreateService(Service service, Guid requestId, IResponder responder) {
            var client = GetServiceClient(service);
            var parameters = new CreateContainerParameters();
            var message = "Successfully created container.";
            var result = DockerResponse.Ok;

            try {
                // SetParameters(parameters, service.Options);
                Console.WriteLine("Image");
                Console.WriteLine(parameters.Image);
                Console.WriteLine("Name");
                Console.WriteLine(parameters.Name);
                await client.Containers.CreateContainerAsync(parameters);
            } catch (Exception exception) {
                message = $"Failed to create container with name {service.Name}: {exception.Message}";
                result = DockerResponse.Failed;
            }

            responder.SendResponse(CreateResponse(requestId, result, message));
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
            
            responder.SendResponse(CreateResponse(requestId, result, message));
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
            
            responder.SendResponse(CreateResponse(requestId, result, message));
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
            
            responder.SendResponse(CreateResponse(requestId, result, message));
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
            
            responder.SendResponse(CreateResponse(requestId, result, message));
        }
        
        #endregion

        private static DockerClient GetServiceClient(Service service) {
            var uri = new Uri($"http://{service.Ip}:{DefaultDockerApiPort}");
            return new DockerClientConfiguration(uri).CreateClient();
        }
        
        private static Response<DockerResponse> CreateResponse(Guid requestId, DockerResponse result, string? message) {
            return new Response<DockerResponse> {
                Guid = requestId,
                Result = result,
                Message = message
            };
        }

        private static void SetParameters<TParameters>(TParameters parameters, List<Option> options) where TParameters : notnull {
            foreach (var option in options) {
                var property = parameters.GetType().GetProperty(option.Name);
                SetParameter(property, parameters, option);
            }
        }

        #region SetParameterValue
        
        private static void SetParameter<TParameters>(PropertyInfo? property, TParameters parameters, Option option) {
            if (option.Type == typeof(bool).ToString())
                DeserializeAndSetValue<bool, TParameters>(property, parameters, option.Value);
            else if (option.Type == typeof(uint).ToString())
                DeserializeAndSetValue<uint, TParameters>(property, parameters, option.Value);
            else if (option.Type == typeof(string).ToString())
                DeserializeAndSetValue<string, TParameters>(property, parameters, option.Value);
            else if (option.Type == typeof(HostConfig).ToString())
                DeserializeAndSetValue<HostConfig, TParameters>(property, parameters, option.Value);
            else if (option.Type == typeof(HealthConfig).ToString())
                DeserializeAndSetValue<HealthConfig, TParameters>(property, parameters, option.Value);
            else if (option.Type == typeof(NetworkingConfig).ToString())
                DeserializeAndSetValue<NetworkingConfig, TParameters>(property, parameters, option.Value);
            else if (option.Type == typeof(IList<string>).ToString())
                DeserializeAndSetValue<IList<string>, TParameters>(property, parameters, option.Value);
            else if (option.Type == typeof(IDictionary<string, string>).ToString()) 
                DeserializeAndSetValue<IDictionary<string, string>, TParameters>(property, parameters, option.Value);
            else if (option.Type == typeof(IDictionary<string, EmptyStruct>).ToString()) 
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