using System.Threading.Tasks;
using AuthenticationService.Models;
using OrpiLibrary.Models.Common;

namespace AuthenticationService.Interfaces {
    public interface IUsersWorker
    {
        public Task<User?> GetUser(string login);
        public Task<bool> AddUser(RegistrationModel user);
        public Task<bool> CheckUserExistence(string login);
        public Task<string?> GetUserPassword(string login);
    }
}