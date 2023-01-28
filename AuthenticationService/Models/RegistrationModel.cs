using System.ComponentModel.DataAnnotations;

namespace AuthenticationService.Models {
    public class RegistrationModel {
        [Required] public string Login { get; set; } = null!;
        [Required] public string Password { get; set; } = null!;
        [Required] public string Name { get; set; } = null!;
        [Required] public string Surname { get; set; } = null!;
        [Required] [EmailAddress] public string Email { get; set; } = null!;
    }
}