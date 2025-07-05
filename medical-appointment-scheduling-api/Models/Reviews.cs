using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace medical_appointment_scheduling_api.Models
{
    public class Reviews
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [ForeignKey("Appointment")]
        [Column("appointment_id")]
        public int AppointmentId { get; set; }

        [Required]
        [Column("rating")]
        public int Rating { get; set; }

        [MaxLength(255)]
        [Column("comment")]
        public string? Comment { get; set; }

        [Column("created_at")]
        public DateTime CreatedAt { get; set; }
    }
}
