using System.ComponentModel.DataAnnotations;

namespace medical_appointment_scheduling_api.Models
{
    public class LoginUser
    {
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
