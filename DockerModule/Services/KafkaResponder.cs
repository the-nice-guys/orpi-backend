using System;
using DockerModule.Interfaces;
using JetBrains.Annotations;
using OrpiLibrary.Models.Docker.Enums;

namespace DockerModule.Services {
    [UsedImplicitly]
    public class KafkaResponder : IResponder {
        public void SendResponse(DockerResponseType response, string? message) { 
            Console.ForegroundColor = response switch {
                DockerResponseType.Ok => ConsoleColor.Green,
                DockerResponseType.Failed => ConsoleColor.Red,
                
                _ => ConsoleColor.White
            }; 
            
            Console.WriteLine($"{response}: {message}");
            Console.ForegroundColor = ConsoleColor.Black;
        }
    }
}