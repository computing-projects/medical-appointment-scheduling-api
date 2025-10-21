using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace medical_appointment_scheduling_api.Models
{
    public class Schedules
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [ForeignKey("Doctor")]
        [Column("doctor_id")]
        public int DoctorId { get; set; }

        [ForeignKey("Clinic")]
        [Column("clinic_id")]
        public int ClinicId { get; set; }

        [Required]
        [Column("weekday")]
        public SystemEnums.Weekday Weekday { get; set; }

        [Required]
        [Column("start_time")]
        public TimeOnly StartTime { get; set; }

        [Required]
        [Column("end_time")]
        public TimeOnly EndTime { get; set; }

        [Column("available")]
        public bool Available { get; set; } = true;
    }
}
