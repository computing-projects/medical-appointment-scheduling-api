
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static medical_appointment_scheduling_api.Models.SystemEnums;
using System.Text.Json.Serialization;

namespace medical_appointment_scheduling_api.Models
{
    public class Doctors
    {
        [Key]
        public int id { get; set; }

        [ForeignKey(nameof(Users))]
        public int user_id { get; set; }

        [Required, MaxLength(20)]
        public string crm { get; set; }

        [Required, Column(TypeName = "varchar(255)")]
        public SystemEnums.Speciality specialty { get; set; }

        public string? accepted_health_plans { get; set; }

        [Required]
        public int appointment_duration_min { get; set; }
    }
}
