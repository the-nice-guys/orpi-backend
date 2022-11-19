using Dapper;
using infrastructure_service.Interfaces;
using infrastructure_service.Models;
using Npgsql;

namespace infrastructure_service.Services;

public class ServiceRepository: IServiceRepository
{
    private readonly string _connectionString;
    
    public ServiceRepository(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection");
    }
    
    public async Task<Service> Get(long id)
    {
        await using var connection = new NpgsqlConnection(_connectionString);
        await connection.OpenAsync();
        var command = new NpgsqlCommand("SELECT * FROM services WHERE id = @id", connection);
        var queryParameters = new
        {
            id = id
        };
        return await connection.QueryFirstOrDefaultAsync<Service>(command.CommandText, queryParameters);
    }

    public async Task<bool> Update(Service service)
    {
        await using var connection = new NpgsqlConnection(_connectionString);
        await connection.OpenAsync();
        var command = new NpgsqlCommand("UPDATE services SET name = @name, description = @description WHERE id = @id", connection);
        var queryParameters = new
        {
            id = service.Id,
            name = service.Name,
            description = service.Description
        };
        return await connection.ExecuteAsync(command.CommandText, queryParameters) > 0;
    }

    public async Task<bool> Delete(long id)
    {
        await using var connection = new NpgsqlConnection(_connectionString);
        await connection.OpenAsync();
        var command = new NpgsqlCommand("DELETE FROM services WHERE id = @id", connection);
        var queryParameters = new
        {
            id = id
        };
        return await connection.ExecuteAsync(command.CommandText, queryParameters) > 0;
    }

    public async Task<long> Create(Service service)
    {
        await using var connection = new NpgsqlConnection(_connectionString);
        await connection.OpenAsync();
        var command = new NpgsqlCommand("INSERT INTO services (name, description) VALUES (@name, @description) RETURNING id", connection);
        var queryParameters = new
        {
            name = service.Name,
            description = service.Description
        };
        return await connection.QuerySingleAsync<long>(command.CommandText, queryParameters);
    }
}