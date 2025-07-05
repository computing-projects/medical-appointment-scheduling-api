
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static medical_appointment_scheduling_api.Models.SystemEnums;
using System.Text.Json.Serialization;

namespace medical_appointment_scheduling_api.Models
{
    public class Doctors
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [ForeignKey("User")]
        [Column("user_id")]
        public int UserId { get; set; }

        [Required, MaxLength(20)]
        [Column("crm")]
        public string Crm { get; set; }

        [Required]
        [Column("specialty")]
        public SystemEnums.Speciality Specialty { get; set; }

        [Required]
        [Column("appointment_duration_med")]
        public int AppointmentDurationMed { get; set; }
    }
}
