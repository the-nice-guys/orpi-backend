using Dapper;
using infrastructure_service.Interfaces;
using Npgsql;
using Host = infrastructure_service.Models.Host;

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

    public async Task<long> Create(Host host)
    {
        await using var connection = new NpgsqlConnection(_connectionString);
        await connection.OpenAsync();
        await using var command = new NpgsqlCommand("INSERT INTO hosts (name, descriprion, icon, ip) VALUES (@name, @description, @icon, @ip) RETURNING id", connection);
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