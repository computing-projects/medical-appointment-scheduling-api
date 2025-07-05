using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace medical_appointment_scheduling_api.Models
{
    public class Notifications
    {
        [Key]
        public int id { get; set; }

        [ForeignKey(nameof(Users))]
        public int user_id { get; set; }

        [Required]
        public string message { get; set; }

        [Required, Column(TypeName = "varchar(50)")]
        public SystemEnums.NotificationType type { get; set; }

        public bool sent { get; set; } = false;

        public DateTime created_at { get; set; } = DateTime.UtcNow;
    }
}
