using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using OrpiLibrary.Models.Common.Responses;
using OrpiLibrary.Models.Docker.Enums;

namespace DockerModule.Services;

public interface IUpdateRepository
{
    Task PushUpdate(
        Guid requestId,
        string serverIpAddress,
        DockerRequest updateType,
        string requestPayload,
        CancellationToken cancellationToken);

    Task<IReadOnlyList<PullUpdateResponse>> PullUpdates(
        string serverIpAddress,
        CancellationToken cancellationToken);
}
