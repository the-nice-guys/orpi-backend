using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OrpiLibrary.Interfaces;

namespace AuthenticationService.Controllers {
    [ApiController]
    [Route("/account/")]
    
    public class AccountController: Controller {
        private readonly ICryptographer _cryptographer;
        
        public AccountController(ICryptographer cryptographer) {
            _cryptographer = cryptographer;
        }
        
        [HttpPost("register")]
        public IActionResult Register() {
            return Ok();
        }

        [HttpPost("auth")]
        public IActionResult Authorize() {
            return Ok();
        }

        [Authorize]
        [HttpGet("refresh")]
        public IActionResult Refresh() {
            return Ok();
        }
    }
}