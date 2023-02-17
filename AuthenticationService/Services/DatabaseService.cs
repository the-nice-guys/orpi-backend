using System.Linq;
using System.Threading.Tasks;
using AuthenticationService.Interfaces;
using AuthenticationService.Models;
using Dapper;
using Npgsql;
using OrpiLibrary.Models.Common;

namespace AuthenticationService.Services {
    public class DatabaseService: IUsersWorker {
        private readonly string _connectionString;

        public DatabaseService(string host, int port, string database, string user, string password) {
            _connectionString = $"Host={host};Port={port};Database={database};User ID={user};Password={password}";
        }
        
        public async Task<bool> AddUser(RegistrationModel user) {
            const string request = "INSERT INTO users (login, email, name, surname) VALUES (@login, @email, @name, @surname);" +
                                   "INSERT INTO accounts (login, password) VALUES (@login, @password);";
            await using var connection = new NpgsqlConnection(_connectionString);
            await connection.OpenAsync();
            await using var transaction = await connection.BeginTransactionAsync();
            try {
                await connection.ExecuteAsync(request, user);
                await transaction.CommitAsync();
                return true;
            } catch {
                await transaction.RollbackAsync();
            }

            return false;
        }
        
        public async Task<User?> GetUser(string login) {
            const string request = "SELECT * FROM users WHERE login = @login;";
            await using var connection = new NpgsqlConnection(_connectionString);
            await connection.OpenAsync();
            return (await connection.QueryAsync<User>(request, new {login})).FirstOrDefault();
        }

        public async Task<bool> CheckUserExistence(string login) {
            const string request = "SELECT COUNT(1) FROM USERS WHERE login = @login";
            await using var connection = new NpgsqlConnection(_connectionString);
            return (await connection.QueryAsync<int>(request, new {login})).FirstOrDefault() == 1;
        }

        public async Task<string?> GetUserPassword(string login) {
            const string request = "SELECT password FROM accounts WHERE login = @login;";
            await using var connection = new NpgsqlConnection(_connectionString);
            return (await connection.QueryAsync<string?>(request, new {login})).FirstOrDefault();
        }
    }
}