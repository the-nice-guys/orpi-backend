using System.Text.Json;
using Dapper;
using infrastructure_service.Interfaces;
using OrpiLibrary.Models;
using Npgsql;
using Host = OrpiLibrary.Models.Host;

namespace infrastructure_service.Services;

public class HostRepository: IHostRepository
{
    private readonly string _connectionString;
    
    public HostRepository(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection");
    }
    
    public async Task<Host> Get(long id)
    {
        await using var connection = new NpgsqlConnection(_connectionString);
        await connection.OpenAsync();
        await using var command = new NpgsqlCommand("SELECT * FROM hosts WHERE id = @id", connection);
        var queryParameters = new
        {
            id = id
        };
        return await connection.QuerySingleAsync<Host>(command.CommandText, queryParameters);
    }

    public async Task<bool> InsertInfrastructureHost(long infrastructureId, long hostId)
    {
        await using var connection = new NpgsqlConnection(_connectionString);
        await connection.OpenAsync();
        await using var command = new NpgsqlCommand("INSERT INTO infrastructure_host (infrastructure_id, host_id) VALUES (@infrastructureId, @hostId)", connection);
        var queryParameters = new
        {
            infrastructureId = infrastructureId,
            hostId = hostId
        };
        return await connection.ExecuteAsync(command.CommandText, queryParameters) > 0;
    }

    public async Task<IEnumerable<Host>> GetAllForInfrastructure(long infrastructureId)
    {
        await using var connection = new NpgsqlConnection(_connectionString);
        await connection.OpenAsync();
        await using var command = new NpgsqlCommand("select * from hosts h join (select hs.host_id as id, array_to_json(array_agg(s)) as services from host_service as hs join services as s on hs.service_id = s.id group by hs.host_id) as t using (id) where h.id in (select host_id from infrastructure_host where infrastructure_id = 5);", connection);
        var queryParameters = new
        {
            id = infrastructureId
        };
        var result = await connection.QueryAsync(command.CommandText, queryParameters);
        
        var hosts = new List<Host>();
        foreach (var item in result)
        {
            var host = new Host
            {
                Id = item.id,
                Name = item.name,
                Services = JsonSerializer.Deserialize<List<Service>>(item.services as string ?? string.Empty)
            };
            hosts.Add(host);
        }

        return hosts;
    }

    public async Task<long> Create(Host host)
    {
        await using var connection = new NpgsqlConnection(_connectionString);
        await connection.OpenAsync();
        await using var command = new NpgsqlCommand("INSERT INTO hosts (name, description, icon, ip) VALUES (@name, @description, @icon, @ip) RETURNING id", connection);
        var queryParameters = new
        {
            name = host.Name,
            description = host.Description,
            icon = host.Icon,
            ip = host.Ip
        };
        return await connection.QuerySingleAsync<long>(command.CommandText, queryParameters);
    }

    public async Task<bool> Update(Host host)
    {
        await using var connection = new NpgsqlConnection(_connectionString);
        await connection.OpenAsync();
        await using var command = new NpgsqlCommand("UPDATE hosts SET name = @name, description = @description, icon = @icon, ip = @ip WHERE id = @id", connection);
        var queryParameters = new
        {
            id = host.Id,
            name = host.Name,
            description = host.Description,
            icon = host.Icon,
            ip = host.Ip
        };
        return await connection.ExecuteAsync(command.CommandText, queryParameters) > 0;
    }

    public async Task<bool> Delete(long id)
    {
        await using var connection = new NpgsqlConnection(_connectionString);
        await connection.OpenAsync();
        await using var command = new NpgsqlCommand("DELETE FROM hosts WHERE id = @id", connection);
        var queryParameters = new
        {
            id = id
        };
        return await connection.ExecuteAsync(command.CommandText, queryParameters) > 0;
    }
}