using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace medical_appointment_scheduling_api.Models
{
    public class ClientHealthPlans
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [ForeignKey("Client")]
        [Column("client_id")]
        public int ClientId { get; set; }

        [ForeignKey("HealthPlan")]
        [Column("health_plan_id")]
        public int HealthPlanId { get; set; }
    }
}
