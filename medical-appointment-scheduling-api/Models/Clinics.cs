using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace medical_appointment_scheduling_api.Models
{
    public class Clinics
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Required, MaxLength(255)]
        [Column("name")]
        public string Name { get; set; }

        [Required]
        [Column("cep")]
        [RegularExpression(@"^\d{8}$")]
        public string Cep { get; set; }

        [Required]
        [Column("address")]
        public string Address { get; set; }

        [Required, MaxLength(20)]
        [Column("cnpj")]
        public string Cnpj { get; set; }

        [Required, MaxLength(255)]
        [Column("email")]
        public string Email { get; set; }

        [Required, MaxLength(20)]
        [Column("phone")]
        public string Phone { get; set; }

        [MaxLength(255)]
        [Column("website")]
        public string? Website { get; set; }

        [Column("created_at")]
        public DateTimeOffset CreatedAt { get; set; }

        [Column("updated_at")]
        public DateTimeOffset UpdatedAt { get; set; }

        [Column("deleted_at")]
        public DateTimeOffset? DeletedAt { get; set; }
    }
}
