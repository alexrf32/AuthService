using System.ComponentModel.DataAnnotations;

namespace AuthService.DTOs
{
    public class UpdatePasswordDto
    {
        [Required]
        public string Email { get; set; } = null!;
        
        [Required]
        public string CurrentPassword { get; set; } = null!;

        [Required]
        [StringLength(16, MinimumLength = 10)]
        [RegularExpression(@"^(?=.*[A-Za-z])(?=.*\d)[A-Za-z\d@$!%*?&]{10,16}$", ErrorMessage = "Password must have at least one letter and one number")]
        public string Password { get; set; } = null!;

        [Required]
        [Compare(nameof(Password))]
        public string RepeatedPassword { get; set; } = null!;
    }
}
