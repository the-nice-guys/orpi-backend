using Dapper;
using infrastructure_service.Interfaces;
using Npgsql;
using OrpiLibrary.Models;

namespace infrastructure_service.Services;

public class OptionsRepository: IOptionsRepository
{
    private readonly string _connectionString;
    
    public OptionsRepository(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("InfrastructureDatabase");
    }
    
    public async Task<Option> Get(long id)
    {
        await using var connection = new NpgsqlConnection(_connectionString);
        await connection.OpenAsync();
        var command = new NpgsqlCommand("SELECT * FROM options WHERE id = @id", connection);
        var queryParameters = new
        {
            id = id
        };
        return await connection.QueryFirstOrDefaultAsync<Option>(command.CommandText, queryParameters);
    }

    public async Task<bool> InsertServiceOption(long serviceId, long optionId)
    {
        await using var connection = new NpgsqlConnection(_connectionString);
        await connection.OpenAsync();
        var command = new NpgsqlCommand("INSERT INTO service_options (service_id, option_id) VALUES (@serviceId, @optionId)", connection);
        var queryParameters = new
        {
            serviceId = serviceId,
            optionId = optionId
        };
        return await connection.ExecuteAsync(command.CommandText, queryParameters) > 0;
    }

    public async Task<IEnumerable<Option>> GetAllForService(long serviceId)
    {
        await using var connection = new NpgsqlConnection(_connectionString);
        await connection.OpenAsync();
        var command = new NpgsqlCommand("select o.id, o.name, o.type, o.value from services s join service_options so on s.id = so.service_id join options o on o.id = so.option_id where s.id = @serviceId;", connection);
        var queryParameters = new
        {
            serviceId = serviceId
        };
        var result = await connection.QueryAsync(command.CommandText, queryParameters);
        var list = new List<Option>();
        foreach (var item in result)
        {
            list.Add(new Option
            {
                Id = item.id,
                Name = item.name,
                Type = item.type,
                Value = item.value
            });
        }
        return list;
    }

    public async Task<bool> Update(Option options)
    {
        await using var connection = new NpgsqlConnection(_connectionString);
        await connection.OpenAsync();
        var command = new NpgsqlCommand("UPDATE options SET name = @name, type = @type, value = @value WHERE id = @id", connection);
        var queryParameters = new
        {
            id = options.Id,
            name = options.Name,
            type = options.Type,
            value = options.Value
        };
        return await connection.ExecuteAsync(command.CommandText, queryParameters) > 0;
    }

    public async Task<bool> Delete(long id)
    {
        await using var connection = new NpgsqlConnection(_connectionString);
        await connection.OpenAsync();
        var command = new NpgsqlCommand("DELETE FROM options WHERE id = @id", connection);
        var queryParameters = new
        {
            id = id
        };
        return await connection.ExecuteAsync(command.CommandText, queryParameters) > 0;
    }

    public async Task<long> Create(Option option)
    {
        await using var connection = new NpgsqlConnection(_connectionString);
        await connection.OpenAsync();
        var command = new NpgsqlCommand("INSERT INTO options (name, type, value) VALUES (@name, @type, @value) RETURNING id", connection);
        var queryParameters = new
        {
            name = option.Name,
            type = option.Type,
            value = option.Value
        };
        return await connection.ExecuteScalarAsync<long>(command.CommandText, queryParameters);
    }
}