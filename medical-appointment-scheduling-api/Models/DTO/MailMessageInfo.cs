namespace medical_appointment_scheduling_api.Models.DTO
{
    public class MailMessageInfo
    {
        public Doctors Doctor { get; set; }
        public Clients Client { get; set; }
        public DateTime AppointmentDate { get; set; }
        public Clinics Clinic { get; set; }
    }
}
