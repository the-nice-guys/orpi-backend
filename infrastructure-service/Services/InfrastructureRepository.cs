using System.Text.Json;
using Dapper;
using infrastructure_service.Interfaces;
using infrastructure_service.Models;
using Npgsql;
using Host = infrastructure_service.Models.Host;

namespace infrastructure_service.Services;

public class InfrastructureRepository: IInfrastructureRepository
{
    private string _connectionString;

    public InfrastructureRepository(string connectionString)
    {
        _connectionString = connectionString;
    }
    
    public async Task<long> Create(Infrastructure infrastructure)
    {
        await using var connection = new NpgsqlConnection(_connectionString);
        await connection.OpenAsync();
        await using var command = new NpgsqlCommand("INSERT INTO infrastructures (name, description, icon) VALUES (@name, @description, @icon) returning id", connection);
        var queryParameters = new
        {
            name = infrastructure.Name,
            description = infrastructure.Description,
            icon = infrastructure.Icon
        };
        // await connection.ExecuteAsync(command.CommandText, queryParameters);
        return await connection.QuerySingleAsync<long>(command.CommandText, queryParameters);
    }

    public async Task<bool> InsertUserInfrastructure(long userId, long infrastructureId)
    {
        await using var connection = new NpgsqlConnection(_connectionString);
        await connection.OpenAsync();
        await using var command = new NpgsqlCommand("INSERT INTO user_infrastructures (user_id, infrastructure_id) VALUES (@userId, @infrastructureId)", connection);
        var queryParameters = new
        {
            userId,
            infrastructureId
        };
        await connection.ExecuteAsync(command.CommandText, queryParameters);
        return true;
    }

    public async Task<bool> Update(Infrastructure infrastructure)
    {
        await using var connection = new NpgsqlConnection(_connectionString);
        await connection.OpenAsync();
        await using var command = new NpgsqlCommand("UPDATE infrastructures SET name = @name, description = @description WHERE id = @id", connection);
        var queryParameters = new
        {
            id = infrastructure.Id,
            name = infrastructure.Name,
            description = infrastructure.Description
        };
        await connection.ExecuteAsync(command.CommandText, queryParameters);
        return true;
    }

    public async Task<bool> Delete(long id)
    {
        await using var connection = new NpgsqlConnection(_connectionString);
        await connection.OpenAsync();
        await using var command = new NpgsqlCommand("DELETE FROM infrastructures WHERE id = @id", connection);
        var queryParameters = new
        {
            id = id
        };
        await connection.ExecuteAsync(command.CommandText, queryParameters);
        return true;
    }

    public async Task<Infrastructure> Get(long id)
    {
        await using var connection = new NpgsqlConnection(_connectionString);
        await connection.OpenAsync();
        await using var command = new NpgsqlCommand("select id, name, description, icon, h5 as hosts from infrastructures i join (select ih.infrastructure_id as id, array_to_json(array_agg(h)) as h5 from infrastructure_host as ih join (select hs.host_id as host_id, h3.name, h3.description, h3.icon, h3.ip, array_agg(s) as services from host_service as hs join hosts h3 on hs.host_id = h3.id join services s on hs.service_id = s.id group by h3.description, h3.name, hs.host_id, h3.icon, h3.ip) as h using (host_id) group by ih.infrastructure_id) as t using (id) where i.id = @id;", connection);
        var queryParameters = new
        {
            id = id
        };
        var result =
            await connection.QueryFirstAsync(command.CommandText, queryParameters) as IDictionary<string, object>;

        var infrastructure = new Infrastructure
        {
            Id = result?["id"] as int? ?? 0,
            Name = result?["name"] as string,
            Description = result?["description"] as string,
            Icon = result?["icon"] as string,
            Hosts = JsonSerializer.Deserialize<List<Host>>(result?["hosts"] as string ?? string.Empty)
        };
        return infrastructure;
    }

    public async Task<IEnumerable<Infrastructure>> GetAllForUser(long userId)
    {
        await using var connection = new NpgsqlConnection(_connectionString);
        await connection.OpenAsync();
        await using var command = new NpgsqlCommand("select * from infrastructures i left join (select ih.infrastructure_id as id, array_to_json(array_agg(h)) as hosts from infrastructure_host as ih left join (select hs.host_id as host_id, h3.name, h3.description, h3.icon, h3.ip, array_agg(s) as services from host_service as hs left join hosts h3 on hs.host_id = h3.id left join services s on hs.service_id = s.id group by h3.description, h3.name, hs.host_id, h3.icon, h3.ip) as h using (host_id) group by ih.infrastructure_id) as t using (id) where i.id in (select infrastructure_id from user_infrastructures where user_id = @userId);", connection);
        var queryParameters = new
        {
            userId = userId
        };
        var result = await connection.QueryAsync(command.CommandText, queryParameters);
        var infrastructures = new List<Infrastructure>();
        foreach (var item in result)
        {
            var infrastructure = new Infrastructure
            {
                Id = item.id,
                Name = item.name,
                Description = item.description,
                Icon = item.icon,
                Hosts = JsonSerializer.Deserialize<List<Host>>(item.hosts as string ?? string.Empty)
            };
            infrastructures.Add(infrastructure);
        }
        return infrastructures;
    }
}