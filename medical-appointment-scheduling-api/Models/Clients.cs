using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace medical_appointment_scheduling_api.Models
{
    public class Clients
    {
        [Key]
        public int id { get; set; }

        [ForeignKey(nameof(Users))]
        public int user_id { get; set; }

        [Required, MaxLength(20)]
        public string rg { get; set; }

        [Required, MaxLength(14)]
        public string cpf { get; set; }

        [MaxLength(255)]
        public string? health_plan { get; set; }

        public string? health_history { get; set; }

        [Required]
        public DateTime date_of_birth { get; set; }
    }
}
