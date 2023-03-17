using System.Threading.Tasks;
using OrpiLibrary.Models.Common;
using OrpiLibrary.Models.Docker.Enums;

namespace DockerModule.Interfaces;
    
public interface IResponder { 
    public Task SendResponse(Response<DockerResponse> response);
}