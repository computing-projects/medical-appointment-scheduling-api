using System.ComponentModel.DataAnnotations;

namespace medical_appointment_scheduling_api.Models
{
    public class Clinics
    {
        [Key]
        public int id { get; set; }

        [Required, MaxLength(255)]
        public string name { get; set; }

        [Required]
        public string address { get; set; }

        [Required, MaxLength(20)]
        public string cnpj { get; set; }

        [Required, MaxLength(255)]
        public string email { get; set; }

        [Required, MaxLength(20)]
        public string phone { get; set; }

        [MaxLength(255)]
        public string? website { get; set; }

        public DateTime created_at { get; set; } = DateTime.UtcNow;
    }
}
