using System.ComponentModel.DataAnnotations;

namespace medical_appointment_scheduling_api.Models.DTO
{
    public class LoginRequest
    {
        [Required]
        public string Email { get; set; }
        
        [Required]
        public string Password { get; set; }
    }
}

