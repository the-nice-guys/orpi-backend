using OrpiLibrary.Models.Common;
using OrpiLibrary.Models.Docker.Enums;

namespace DockerModule.Interfaces;
    
public interface IResponder { 
    public void SendResponse(Response<DockerResponse> response);
}