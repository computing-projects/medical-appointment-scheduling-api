using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace medical_appointment_scheduling_api.Models
{
    public class Anamnese
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [ForeignKey("Client")]
        [Column("client_id")]
        public int ClientId { get; set; }

        [Column("medical_history")]
        public string? MedicalHistory { get; set; }

        [Column("allergies")]
        public string? Allergies { get; set; }

        [Column("notes")]
        public string? Notes { get; set; }

        [Column("created_at")]
        public DateTimeOffset CreatedAt { get; set; }

        [Column("updated_at")]
        public DateTimeOffset UpdatedAt { get; set; }
    }
}
