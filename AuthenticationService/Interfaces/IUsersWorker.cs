using System.Threading.Tasks;
using AuthenticationService.Models;

namespace AuthenticationService.Interfaces {
    public interface IUsersWorker {
        public Task<bool> AddUser(RegistrationModel user);
        public Task<string?> GetUserPassword(string login);
    }
}