using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace medical_appointment_scheduling_api.Models
{
    public class Reviews
    {
        [Key]
        public int id { get; set; }

        [ForeignKey(nameof(Appointments))]
        public int appointment_id { get; set; }

        [Required]
        [Range(1, 5)]
        public int rating { get; set; }

        public string? comment { get; set; }

        public DateTime created_at { get; set; } = DateTime.UtcNow;
    }
}
