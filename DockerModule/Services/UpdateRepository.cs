using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Extensions.Options;
using Npgsql;
using OrpiLibrary.Models.Common.Responses;
using OrpiLibrary.Models.Docker.Enums;

namespace DockerModule.Services;

public class UpdateRepository: IUpdateRepository
{
    private readonly UpdateRepositoryConfig _config;

    public UpdateRepository(IOptions<UpdateRepositoryConfig> config)
    {
        _config = config.Value;
    }

    public async Task PushUpdate(
        Guid requestId,
        string serverIpAddress,
        DockerRequest updateType,
        string requestPayload,
        CancellationToken cancellationToken)
    {
        const string request =
            "INSERT INTO updates (request_id, server_ip_address, update_type, payload, is_deleted, created_on) " +
            "VALUES (@requestId, @serverIpAddress, @updateType, @payload, @isDeleted, @CcreatedOn)";

        var values = new
        {
            requestId = requestId.ToString(),
            serverIpAddress = serverIpAddress,
            updateType = (int)updateType,
            paylod = requestPayload,
            isDeleted = false,
            createdOn = DateTime.Now,
        };

        await using var connection = new NpgsqlConnection(_config.ConnectionString);
        await connection.OpenAsync(cancellationToken);
        await connection.ExecuteAsync(request, values);
    }

    public async Task<IReadOnlyList<PullUpdateResponse>> PullUpdates(
        string serverIpAddress,
        CancellationToken cancellationToken)
    {
        const string request =
            "UPDATE updates " +
            "SET is_deleted = true " +
            "WHERE server_ip_address = @serverIpAddress AND NOT is_deleted " +
            "RETURNING payload, update_type, created_on";
        
        await using var connection = new NpgsqlConnection(_config.ConnectionString);
        await connection.OpenAsync(cancellationToken);
        return (await connection.QueryAsync<PullUpdateResponse>(request, serverIpAddress)).ToList();
    }
}
