using Dapper;
using infrastructure_service.Interfaces;
using OrpiLibrary.Models;
using Npgsql;

namespace infrastructure_service.Services;

public class ServiceRepository: IServiceRepository
{
    private readonly string _connectionString;
    
    public ServiceRepository(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("InfrastructureDatabase");
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

    public async Task<bool> InsertHostService(long hostId, long serviceId)
    {
        await using var connection = new NpgsqlConnection(_connectionString);
        await connection.OpenAsync();
        var command = new NpgsqlCommand("INSERT INTO host_service (host_id, service_id) VALUES (@hostId, @serviceId)", connection);
        var queryParameters = new
        {
            hostId = hostId,
            serviceId = serviceId
        };
        return await connection.ExecuteAsync(command.CommandText, queryParameters) > 0;
    }

    public async Task<IEnumerable<Service>> GetAllForInfrastructure(long infrastructureId)
    {
        await using var connection = new NpgsqlConnection(_connectionString);
        await connection.OpenAsync();
        var command = new NpgsqlCommand("select s.id, s.name, s.description, s.\"lastUpdated\" from infrastructures as i left outer join infrastructure_host as ih on i.id = ih.infrastructure_id left outer join hosts as h on ih.host_id = h.id left outer join host_service hs on h.id = hs.host_id left outer join services s on hs.service_id = s.id where i.id = 5;", connection);
        var queryParameters = new
        {
            id = infrastructureId
        };
        var result = await connection.QueryAsync(command.CommandText, queryParameters);
        var list = new List<Service>();
        foreach (var item in result)
        {
            list.Add(new Service
            {
                Id = item.id,
                Name = item.name,
                Description = item.description,
                LastUpdated = item.lastUpdated
            });
        }
        return list;
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