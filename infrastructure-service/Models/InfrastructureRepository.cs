using Dapper;
using Npgsql;

namespace infrastructure_service.Models;

public class InfrastructureRepository: IInfrastructureRepository
{
    private string _connectionString;

    public InfrastructureRepository(string connectionString)
    {
        _connectionString = connectionString;
    }
    
    public async Task Create(Infrastructure infrastructure)
    {
        await using var connection = new NpgsqlConnection(_connectionString);
        await connection.OpenAsync();
        await using var command = new NpgsqlCommand("INSERT INTO infrastructure (name, description) VALUES (@name, @description)", connection);
        var queryParameters = new
        {
            name = infrastructure.Name,
            description = infrastructure.Description
        };
        await connection.ExecuteAsync(command.CommandText, queryParameters);
    }

    public async Task Update(Infrastructure infrastructure)
    {
        await using var connection = new NpgsqlConnection(_connectionString);
        await connection.OpenAsync();
        await using var command = new NpgsqlCommand("UPDATE infrastructure SET name = @name, description = @description WHERE id = @id", connection);
        var queryParameters = new
        {
            id = infrastructure.Id,
            name = infrastructure.Name,
            description = infrastructure.Description
        };
        await connection.ExecuteAsync(command.CommandText, queryParameters);
    }

    public async Task Delete(Infrastructure infrastructure)
    {
        await using var connection = new NpgsqlConnection(_connectionString);
        await connection.OpenAsync();
        await using var command = new NpgsqlCommand("DELETE FROM infrastructure WHERE id = @id", connection);
        var queryParameters = new
        {
            id = infrastructure.Id
        };
        await connection.ExecuteAsync(command.CommandText, queryParameters);
    }

    public async Task<Infrastructure> Get(long id)
    {
        await using var connection = new NpgsqlConnection(_connectionString);
        await connection.OpenAsync();
        await using var command = new NpgsqlCommand("SELECT * FROM infrastructure WHERE id = @id", connection);
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
        await using var command = new NpgsqlCommand("SELECT * FROM infrastructure WHERE user_id = @userId", connection);
        var queryParameters = new
        {
            userId = userId
        };
        var result = await connection.QueryAsync<Infrastructure>(command.CommandText, queryParameters);
        return result;
    }
}