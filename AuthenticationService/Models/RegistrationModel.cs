using System.ComponentModel.DataAnnotations;

namespace AuthenticationService.Models {
    public class RegistrationModel {
        [Required] 
        public string Login { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Surname { get; set; }
    }
}