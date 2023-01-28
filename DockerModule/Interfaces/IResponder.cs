using OrpiLibrary.Models.Docker.Enums;

namespace DockerModule.Interfaces {
    public interface IResponder {
        public void SendResponse(DockerResponseType response, string? message);
    }
}