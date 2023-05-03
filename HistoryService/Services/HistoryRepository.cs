using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Dapper;
using HistoryService.Interfaces;
using Microsoft.Extensions.Configuration;
using Npgsql;
using OrpiLibrary.Models;

namespace HistoryService.Services;

public class HistoryRepository: IHistoryRepository
{
    private readonly string _connectionString;
    
    public HistoryRepository(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("HistoryDatabase");
    }
    
    public async Task<IEnumerable<HistoryLog>> GetHistory(long infrastructureId, long take)
    {
        await using var connection = new NpgsqlConnection(_connectionString);
        await connection.OpenAsync();
        await using var command = new NpgsqlCommand("SELECT * FROM history WHERE infrastructure_id = @id LIMIT @take", connection);
        var queryParameters = new
        {
            id = infrastructureId,
            take = take
        };
        return await connection.QueryAsync<HistoryLog>(command.CommandText, queryParameters);
    }

    public async Task WriteHistory(long infrastructureId, HistoryLog log)
    {
        await using var connection = new NpgsqlConnection(_connectionString);
        await connection.OpenAsync();
        await using var command = new NpgsqlCommand("INSERT INTO history (infrastructure_id, \"timestamp\", title, message) VALUES (@id, @timestamp, @title, @message)", connection);
        var queryParameters = new
        {
            id = infrastructureId,
            timestamp = DateTime.UtcNow,
            title = log.Title,
            message = log.Message
        };
        await connection.ExecuteAsync(command.CommandText, queryParameters);
    }
}