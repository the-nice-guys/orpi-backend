using System.IO;
using Microsoft.Extensions.Hosting;
using Npgsql;
using OrpiLibrary;

namespace AuthenticationService.Extensions {
    public static class DatabaseExtension {
        private static readonly string ConnectionString;
        private const string UsersTableSchema = "Schemas/Users.sql";
        private const string AccountsTableSchema = "Schemas/Accounts.sql";

        static DatabaseExtension() {
            ConnectionString = 
                $"Host={Config.AuthenticationServiceDatabaseHost};" +
                $"Port={Config.AuthenticationServiceDatabasePort};" +
                $"Database={Config.AuthenticationServiceDatabaseName};" +
                $"User ID={Config.AuthenticationServiceDatabaseUser};" +
                $"Password={Config.AuthenticationServiceDatabasePassword}";
        }
        public static IHost MigrationUp(this IHost host) {
            using var connection = new NpgsqlConnection(ConnectionString);
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