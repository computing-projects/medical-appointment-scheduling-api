namespace medical_appointment_scheduling_api.Models.DTO
{
    public class UserData
    {
        public int UserId { get; set; }
        public int? MedicId { get; set; }
        public int? ClientId { get; set; }
        public SystemEnums.EUserRole UserRole { get; set; }
        public bool IsMedico => MedicId.HasValue;
        public bool IsPaciente => ClientId.HasValue;

    }
}
