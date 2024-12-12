using System.ComponentModel.DataAnnotations;

namespace AuthService.DTOs
{
    public class RegisterStudentDto
    {
        [Required]
        [StringLength(50, MinimumLength = 3)]
        public string Name { get; set; } = null!;

        [Required]
        [StringLength(30, MinimumLength = 3)]
        public string FirstLastName { get; set; } = null!;

        [Required]
        [StringLength(30, MinimumLength = 3)]
        public string SecondLastName { get; set; } = null!;

        [Required]
        [RegularExpression(@"^\d{1,2}\.\d{3}\.\d{3}-[0-9kK]{1}$", ErrorMessage = "Invalid RUT format")]
        public string RUT { get; set; } = null!;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = null!;

        [Required]
        public int CareerId { get; set; }

        [Required]
        [StringLength(16, MinimumLength = 10)]
        [RegularExpression(@"^(?=.*[A-Za-z])(?=.*\d)[A-Za-z\d@$!%*?&]{10,16}$", ErrorMessage = "Password must have at least one letter and one number")]
        public string Password { get; set; } = null!;

        [Required]
        [Compare(nameof(Password))]
        public string RepeatedPassword { get; set; } = null!;
    }
}
