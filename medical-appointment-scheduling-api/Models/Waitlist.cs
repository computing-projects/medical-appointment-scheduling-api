using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace medical_appointment_scheduling_api.Models
{
    public class Waitlist
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [ForeignKey("Appointment")]
        [Column("appointment_id")]
        public int AppointmentId { get; set; }

        [ForeignKey("Client")]
        [Column("interested_client_id")]
        public int InterestedClientId { get; set; }

        [Required]
        [Column("status")]
        public SystemEnums.WaitlistStatus Status { get; set; }

        [Column("created_at")]
        public DateTime CreatedAt { get; set; }

        [Column("updated_at")]
        public DateTime UpdatedAt { get; set; }
    }
}
