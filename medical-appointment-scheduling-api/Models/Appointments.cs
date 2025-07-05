using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace medical_appointment_scheduling_api.Models
{
    public class Appointments
    {
        [Key]
        public int id { get; set; }

        [ForeignKey(nameof(Clinics))]
        public int clinic_id { get; set; }

        [ForeignKey(nameof(Doctors))]
        public int doctor_id { get; set; }

        [ForeignKey(nameof(Clients))]
        public int client_id { get; set; }

        [Required]
        public DateTime appointment_datetime { get; set; }

        [Required, Column(TypeName = "varchar(20)")]
        public SystemEnums.AppointmentType appointment_type { get; set; }

        [Required, Column(TypeName = "varchar(50)")]
        public SystemEnums.AppointmentStatus status { get; set; }

        public string? video_call_link { get; set; }

        public DateTime created_at { get; set; } = DateTime.UtcNow;
    }
}
