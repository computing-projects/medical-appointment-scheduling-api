namespace medical_appointment_scheduling_api.Models.DTO
{
    public class CurrentUserDto
    {
        public int userId { get; set; }
        public string email { get; set; }
        public string name  { get; set; }
        public string role  { get; set; }   
    }
}