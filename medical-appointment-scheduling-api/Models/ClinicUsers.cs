using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace medical_appointment_scheduling_api.Models
{
    public class ClinicUsers
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [ForeignKey("Clinic")]
        [Column("clinic_id")]
        public int ClinicId { get; set; }

        [ForeignKey("User")]
        [Column("user_id")]
        public int UserId { get; set; }

        [Required]
        [Column("role")]
        public SystemEnums.ETipoClinicUser Role { get; set; }
    }
}
