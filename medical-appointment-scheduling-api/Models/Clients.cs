using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace medical_appointment_scheduling_api.Models
{
    public class Clients
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [ForeignKey("User")]
        [Column("user_id")]
        public int UserId { get; set; }

        [Required, MaxLength(14)]
        [Column("cpf")]
        public string Cpf { get; set; }

        [Column("health_plan")]
        public SystemEnums.HealthPlans? HealthPlan { get; set; }

        [Column("health_history")]
        public string? HealthHistory { get; set; }

        [Required]
        [Column("date_of_birth")]
        public DateTime DateOfBirth { get; set; }
    }
}
