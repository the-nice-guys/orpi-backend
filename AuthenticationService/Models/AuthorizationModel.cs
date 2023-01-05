using System.ComponentModel.DataAnnotations;

namespace AuthenticationService.Models {
    public class AuthorizationModel {
        [Required]
        public string Login { get; set; }
        [Required]
        public string Password { get; set; }
    }
}