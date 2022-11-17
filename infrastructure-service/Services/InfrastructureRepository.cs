using Dapper;
using infrastructure_service.Interfaces;
using infrastructure_service.Models;
using Npgsql;

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
        await using var command = new NpgsqlCommand("INSERT INTO infrastructures (name, description) VALUES (@name, @description) returning id", connection);
        var queryParameters = new
        {
            name = infrastructure.Name,
            description = infrastructure.Description
        };
        // await connection.ExecuteAsync(command.CommandText, queryParameters);
        return await connection.QuerySingleAsync<long>(command.CommandText, queryParameters);
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
        await using var command = new NpgsqlCommand("SELECT * FROM infrastructures WHERE id = @id", connection);
        var queryParameters = new
        {
            id = id
        };
        var result = await connection.QueryFirstOrDefaultAsync<Infrastructure>(command.CommandText, queryParameters);
        return result;
    }

    public async Task<IEnumerable<Infrastructure>> GetAllForUser(long userId)
    {
        await using var connection = new NpgsqlConnection(_connectionString);
        await connection.OpenAsync();
        await using var command = new NpgsqlCommand("SELECT * FROM infrastructures WHERE user_id = @userId", connection);
        var queryParameters = new
        {
            userId = userId
        };
        var result = await connection.QueryAsync<Infrastructure>(command.CommandText, queryParameters);
        return result;
    }
}