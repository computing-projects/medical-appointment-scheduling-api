using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace medical_appointment_scheduling_api.Models
{
    public class DoctorHealthPlans
    {
        [Key, ForeignKey(nameof(Doctors))]
        public int doctor_Id { get; set; }

        [Column(TypeName = "varchar(20)")]
        public SystemEnums.HealthPlans healthPlan { get; set; }
    }
}
