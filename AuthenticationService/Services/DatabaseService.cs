using System.Linq;
using System.Threading.Tasks;
using AuthenticationService.Interfaces;
using AuthenticationService.Models;
using Dapper;
using Npgsql;

namespace AuthenticationService.Services {
    public class DatabaseService: IUsersWorker {
        private readonly string _connectionString;

        public DatabaseService(string host, int port, string database, string user, string password) {
            _connectionString = $"Host={host};Port={port};Database={database};User ID={user};Password={password}";
        }
        
        public async Task<bool> AddUser(RegistrationModel user) {
            const string request = "INSERT INTO users (login, email, name, surname) VALUES (@login, @email, @name, @surname);" +
                                   "INSERT INTO accounts (login, password) VALUES (@login, @password);";
            try {
                await using var connection = new NpgsqlConnection(_connectionString);
                await connection.ExecuteAsync(request, user);
            } catch {
                return false;
            }

            return true;
        }

        public async Task<string?> GetUserPassword(string login) {
            const string request = "SELECT password FROM accounts WHERE login = @login;";
            await using var connection = new NpgsqlConnection(_connectionString);
            return (await connection.QueryAsync<string?>(request, new {login})).FirstOrDefault();
        }
    }
}