using System.Threading.Tasks;
using AuthenticationService.Interfaces;
using AuthenticationService.Models;
using AuthenticationService.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OrpiLibrary.Interfaces;
using OrpiLibrary;

namespace AuthenticationService.Controllers {
    [ApiController]
    [Route("/account/")]
    public class AccountController: Controller {
        private readonly IUsersWorker _usersWorker;
        private readonly ITokenCreator _tokenCreator;
        private readonly ICryptographer _cryptographer;
        private readonly TokenDataManager _tokenDataManager;

        public AccountController
            (IUsersWorker usersWorker, ITokenCreator tokenCreator, ICryptographer cryptographer, TokenDataManager tokenDataManager) {
            _usersWorker = usersWorker;
            _tokenCreator = tokenCreator;
            _cryptographer = cryptographer;
            _tokenDataManager = tokenDataManager;
        }
        
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegistrationModel user) {
            user.Password = _cryptographer.Encrypt(user.Password);
            if (await _usersWorker.AddUser(user)) {
                return Ok();
            }

            return BadRequest("Failed to add new user.");
        }

        [HttpPost("auth")]
        public async Task<IActionResult> Authorize([FromBody] AuthorizationModel account) {
            var password = await _usersWorker.GetUserPassword(account.Login);
            if (password is null) {
                return NotFound("User with this login does not exist");
            }

            if (password != _cryptographer.Encrypt(account.Password)) {
                return BadRequest("Incorrect password");
            }
            
            return Ok(new {
                AccessToken = 
                    _tokenCreator.CreateToken(_tokenDataManager.AccessTokenData, account.Login, Roles.User.ToString()),
                RefreshToken = 
                    _tokenCreator.CreateToken(_tokenDataManager.RefreshTokenData, account.Login, Roles.User.ToString()),
            });
        }

        [Authorize]
        [HttpGet("refresh")]
        public IActionResult Refresh() {
            var refreshToken = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            var claims = _tokenDataManager.GetClaims(refreshToken, _tokenDataManager.RefreshTokenData);
            (string login, string role) = (claims[0].Value, claims[1].Value);
            if (_tokenDataManager.GetTimeBeforeExpiration(refreshToken, _tokenDataManager.RefreshTokenData) < Config.AccessTokenLifetime) {
                refreshToken = _tokenCreator.CreateToken(_tokenDataManager.RefreshTokenData, login, role);
            }
            
            return Ok(new {
                AccessToken = _tokenCreator.CreateToken(_tokenDataManager.AccessTokenData, login, role),
                RefreshToken = refreshToken
            });
        }
    }
}