using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace medical_appointment_scheduling_api.Models
{
    public class Schedules
    {
        [Key]
        public int id { get; set; }

        [ForeignKey(nameof(Doctors))]
        public int doctor_id { get; set; }

        [Required, Column(TypeName = "varchar(20)")]
        public SystemEnums.Weekday weekday { get; set; }

        [Required]
        public TimeSpan start_time { get; set; }

        [Required]
        public TimeSpan end_time { get; set; }
    }
}
