using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AuthenticationService.Interfaces;
using AuthenticationService.Models;
using AuthenticationService.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OrpiLibrary;
using OrpiLibrary.Interfaces.Tokens;

namespace AuthenticationService.Controllers {
    [ApiController]
    [Route("/account/")]
    public class AccountController: Controller {
        private const int NumberOfRequiredClaims = 2;
        
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
            try {
                if (await _usersWorker.CheckUserExistence(user.Login))
                    return BadRequest("Account with this login is already registered.");

                if (!await _usersWorker.AddUser(user))
                    return Ok("Failed to add user.");
            } catch {
                return BadRequest("Service is temporarily unavailable.");
            }
            
            return Ok("Account was successfully registered.");
        }


        [HttpPost("auth")]
        public async Task<IActionResult> Authorize([FromBody] AuthorizationModel account) {
            string? password;
            try {
                password = await _usersWorker.GetUserPassword(account.Login);
            } catch {
                return BadRequest("Service is temporarily unavailable.");
            }

            if (password is null) 
                return NotFound("User with this login does not exist");

            if (password != _cryptographer.Encrypt(account.Password))
                return BadRequest("Incorrect password");

            var claims = new List<Claim> {
                new (ClaimsIdentity.DefaultNameClaimType, account.Login),
                new (ClaimsIdentity.DefaultRoleClaimType, Roles.User.ToString()),
            };

            var user = await _usersWorker.GetUser(account.Login);
            
            return Ok(new {
                User = user,
                AccessToken = _tokenCreator.CreateToken(_tokenDataManager.AccessTokenData, claims),
                RefreshToken = _tokenCreator.CreateToken(_tokenDataManager.RefreshTokenData, claims),
            });
        }

        [Authorize]
        [HttpGet("refresh")]
        public IActionResult Refresh() {
            var refreshToken = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            var claims = _tokenDataManager.GetClaims(refreshToken, _tokenDataManager.RefreshTokenData).ToArray();
            claims = claims[..Math.Min(claims.Length, NumberOfRequiredClaims)];
            if (_tokenDataManager.GetTimeBeforeExpiration(refreshToken, _tokenDataManager.RefreshTokenData) < Config.AccessTokenLifetime)
                refreshToken = _tokenCreator.CreateToken(_tokenDataManager.RefreshTokenData, claims);

            return Ok(new {
                AccessToken = _tokenCreator.CreateToken(_tokenDataManager.AccessTokenData, claims),
                RefreshToken = refreshToken
            });
        }
    }
}