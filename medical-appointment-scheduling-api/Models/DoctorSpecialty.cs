using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace medical_appointment_scheduling_api.Models
{
    public class DoctorSpecialty
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [ForeignKey(nameof(Doctors))]
        [Column("doctor_id")]
        public int DoctorId { get; set; }

        [ForeignKey(nameof(Specialty))]
        [Column("specialty_id")]
        public int SpecialtyId { get; set; }
    }
}