using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace medical_appointment_scheduling_api.Models
{
    public class Waitlist
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [ForeignKey("Client")]
        [Column("client_id")]
        public int ClientId { get; set; }

        [ForeignKey("Doctor")]
        [Column("doctor_id")]
        public int DoctorId { get; set; }

        [ForeignKey("Clinic")]
        [Column("clinic_id")]
        public int? ClinicId { get; set; }

        [Required]
        [Column("position")]
        public int Position { get; set; }

        [Required]
        [Column("status")]
        public SystemEnums.WaitlistStatus Status { get; set; }

        [Required]
        [Column("appointment_type")]
        public SystemEnums.AppointmentType AppointmentType { get; set; }

        [Column("reason")]
        public string? Reason { get; set; }

        [Column("created_at")]
        public DateTimeOffset CreatedAt { get; set; }

        [Column("updated_at")]
        public DateTimeOffset UpdatedAt { get; set; }
    }
}
