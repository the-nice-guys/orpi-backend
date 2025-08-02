using OrpiLibrary.Models.Docker.Enums;

namespace OrpiLibrary.Models.Common.Responses;

public class PullUpdateResponse
{
    public Service Service { get; set; } = null!;
    
    public DockerRequest UpdateType { get; set; }
    
    public DateTime CreatedOn { get; set; }
}