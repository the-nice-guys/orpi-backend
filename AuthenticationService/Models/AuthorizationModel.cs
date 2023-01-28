using System.ComponentModel.DataAnnotations;

namespace AuthenticationService.Models {
    public class AuthorizationModel {
        [Required] public string Login { get; set; } = null!;
        [Required] public string Password { get; set; } = null!;
    }
}