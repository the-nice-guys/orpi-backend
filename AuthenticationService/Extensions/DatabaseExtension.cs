using System;
using System.IO;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Npgsql;
using OrpiLibrary;

namespace AuthenticationService.Extensions {
    public static class DatabaseExtension {
        private static string _connectionString = null!;
        private const string UsersTableSchema = "Schemas/Users.sql";
        private const string AccountsTableSchema = "Schemas/Accounts.sql";

        public static void SetConnectionString(IConfiguration configuration) {
            _connectionString = 
                $"Host={configuration["UserDataBase:Host"]};" +
                $"Port={configuration["UserDataBase:Port"]};" +
                $"Database={configuration["UserDataBase:Name"]};" +
                $"User ID={configuration["UserDataBase:User"]};" +
                $"Password={configuration["UserDataBase:Password"]}";

            Console.WriteLine(_connectionString);
        }
        
        public static IHost MigrationUp(this IHost host) {
            using var connection = new NpgsqlConnection(_connectionString);
            connection.Open();
            CreateTable(UsersTableSchema, connection);
            CreateTable(AccountsTableSchema, connection);
            return host;
        }

        private static void CreateTable(string schema, NpgsqlConnection connection) {
            var request = File.ReadAllText(schema);
            new NpgsqlCommand(request, connection).ExecuteNonQuery();
        }
    }
}