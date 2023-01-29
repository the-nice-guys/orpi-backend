using System;
using DockerModule.Interfaces;
using JetBrains.Annotations;
using OrpiLibrary.Models.Common;
using OrpiLibrary.Models.Docker.Enums;

namespace DockerModule.Services;

[UsedImplicitly]
public class KafkaResponder : IResponder {
    public void SendResponse(Response<DockerResponse> response) { 
        Console.ForegroundColor = response.Result switch {
            DockerResponse.Ok => ConsoleColor.Green,
            DockerResponse.Failed => ConsoleColor.Red,
            
            _ => ConsoleColor.White
        }; 
        
        Console.WriteLine($"{response.Result}: {response.Message}");
        Console.ForegroundColor = ConsoleColor.Black;
    }
}