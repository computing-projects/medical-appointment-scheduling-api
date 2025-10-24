namespace medical_appointment_scheduling_api.Models.DTO
{
    public class FiltroMedicos
    {
        public SystemEnums.Speciality specialty { get; set; }
        public bool FilterByHealthPlans { get; set; }
        public DateTimeOffset InitialDate { get; set; }
        public DateTimeOffset FinalDate { get; set; }
    }
}
