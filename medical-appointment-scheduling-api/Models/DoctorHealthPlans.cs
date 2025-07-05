using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace medical_appointment_scheduling_api.Models
{
    public class DoctorHealthPlans
    {
        [Key, Column("doctor_id")]
        public int DoctorId { get; set; }

        [Key, Column("health_plan")]
        public SystemEnums.HealthPlans HealthPlan { get; set; }
    }
}
