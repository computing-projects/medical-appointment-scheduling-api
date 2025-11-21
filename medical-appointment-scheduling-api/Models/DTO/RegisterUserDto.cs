using System.ComponentModel.DataAnnotations;

namespace medical_appointment_scheduling_api.Models.DTO
{
    public class RegisterUserDto
    {
        [Required, MaxLength(255)]
        public string Name { get; set; }

        [Required, MaxLength(255)]
        public string Email { get; set; }

        [Required]
        public string PasswordHash { get; set; }

        [Required]
        [RegularExpression(@"^\d{8}$")]
        public string Cep { get; set; }

        [Required]
        public string Address { get; set; }

        [MaxLength(20)]
        public string? Phone { get; set; }

        [Required]
        public string Role { get; set; }

        [MaxLength(100)]
        public string? City { get; set; }

        [MaxLength(2)]
        public string? State { get; set; }

        [MaxLength(500)]
        public string? ProfilePhotoUrl { get; set; }
    }
}

