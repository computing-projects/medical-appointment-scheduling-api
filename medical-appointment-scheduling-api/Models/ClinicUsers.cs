using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace medical_appointment_scheduling_api.Models
{
    public class ClinicUsers
    {
        [Key]
        public int id { get; set; }

        [ForeignKey(nameof(Clinics))]
        public int clinic_id { get; set; }

        [ForeignKey(nameof(Users))]
        public int user_id { get; set; }

        [Required, Column(TypeName = "varchar(50)")]
        public SystemEnums.ETipoClinicUser role { get; set; }
    }
}
