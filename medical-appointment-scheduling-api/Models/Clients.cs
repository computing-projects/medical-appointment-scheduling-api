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

        [Required, MaxLength(20)]
        [Column("rg")]
        public string Rg { get; set; }

        [Required, MaxLength(14)]
        [Column("cpf")]
        public string Cpf { get; set; }

        [MaxLength(20)]
        [Column("phone")]
        public string? Phone { get; set; }

        [Required]
        [Column("birth_date")]
        public DateTimeOffset BirthDate { get; set; }
    }
}
