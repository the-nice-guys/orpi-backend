using coordinator_service.Intefaces;
using coordinator_service.Models;
using Dapper;
using Npgsql;

namespace coordinator_service.Services;

public class OutboxRepository: IOutboxRepository
{
    private readonly string _connectionString;
    
    public OutboxRepository(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("CoordinatorDatabase");
    }

    public async Task AddOutboxMessage(OutboxMessage message)
    {
        await using var connection = new NpgsqlConnection(_connectionString);
        await connection.OpenAsync();
        var command = new NpgsqlCommand("INSERT INTO outbox (topic, data, state) VALUES (@topic, @data, @state)", connection);
        var queryParameters = new
        {
            topic = message.Topic,
            data = message.Data,
            state = 0
        };
        await connection.ExecuteAsync(command.CommandText, queryParameters);
    }

    public async Task<OutboxMessage> GetUnlockedMassage()
    {
        await using var connection = new NpgsqlConnection(_connectionString);
        await connection.OpenAsync();

        var transaction = await connection.BeginTransactionAsync();
        var command = new NpgsqlCommand("SELECT * FROM outbox WHERE state = 0 FOR UPDATE SKIP LOCKED LIMIT 1", connection);
        var message = await connection.QueryFirstAsync<OutboxMessage>(command.CommandText);
        
        var command2 = new NpgsqlCommand("UPDATE outbox SET state = 1 WHERE id = @id", connection);

        var queryParameters = new
        {
            id = message.Id
        };
        await connection.ExecuteAsync(command2.CommandText, queryParameters);

        await transaction.CommitAsync();
        return message;
    }

    public async Task DeleteMessage(OutboxMessage message)
    {
        await using var connection = new NpgsqlConnection(_connectionString);
        await connection.OpenAsync();
        var command = new NpgsqlCommand("DELETE FROM outbox WHERE id = @id AND state = 1", connection);
        var queryParameters = new
        {
            id = message.Id
        };
        await connection.ExecuteAsync(command.CommandText, queryParameters);
    }
}