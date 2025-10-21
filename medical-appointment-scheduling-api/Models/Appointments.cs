using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace medical_appointment_scheduling_api.Models
{
    public class Appointments
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
        public int ClinicId { get; set; }

        [ForeignKey("Schedule")]
        [Column("schedule_id")]
        public int ScheduleId { get; set; }

        [Required]
        [Column("appointment_datetime")]
        public DateTimeOffset AppointmentDatetime { get; set; }

        [Required]
        [Column("appointment_type")]
        public SystemEnums.AppointmentType AppointmentType { get; set; }

        [Required]
        [Column("status")]
        public SystemEnums.AppointmentStatus Status { get; set; }

        [Column("video_call_link")]
        public string? VideoCallLink { get; set; }

        [Column("created_at")]
        public DateTimeOffset CreatedAt { get; set; }

        [Column("updated_at")]
        public DateTimeOffset UpdatedAt { get; set; }
    }
}
