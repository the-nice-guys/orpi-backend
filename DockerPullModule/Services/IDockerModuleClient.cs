using System.Collections.Generic;
using System.Threading.Tasks;
using OrpiLibrary.Models;
using OrpiLibrary.Models.Common.Responses;

namespace DockerPullModule.Services;

public interface IDockerModuleClient
{
    Task<IReadOnlyList<PullUpdateResponse>> GetUpdates();
}