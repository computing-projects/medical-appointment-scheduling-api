namespace medical_appointment_scheduling_api.Models.DTO
{
    public class FiltroMedicos
    {
        public SystemEnums.Speciality specialty { get; set; }
        public DateTime InitialDate { get; set; }
        public DateTime FinalDate { get; set; }
    }
}
