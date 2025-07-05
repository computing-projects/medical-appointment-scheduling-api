using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace medical_appointment_scheduling_api.Models
{
    public class Waitlist
    {
        [Key]
        public int id { get; set; }

        [ForeignKey(nameof(Appointments))]
        public int appointment_id { get; set; }

        [ForeignKey(nameof(Clients))]
        public int interested_client_id { get; set; }

        [Required, Column(TypeName = "varchar(50)")]
        public SystemEnums.WaitlistStatus status { get; set; }

        public DateTime created_at { get; set; } = DateTime.UtcNow;
    }
}
