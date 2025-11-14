namespace medical_appointment_scheduling_api.Models.DTO
{
    public class ClientProfileDto
    {
        public int Id { get; set; }
        public string? Photo { get; set; }
        public string Name { get; set; }
        public string? Contact { get; set; }
        public int Age { get; set; }
        public string? State { get; set; }
        public string Email { get; set; }
        public string Cpf { get; set; }
        public string? City { get; set; }
        public string Cep { get; set; }
        public List<string> HealthPlans { get; set; } = new();
        public ClientStatisticsDto Statistics { get; set; } = new();
    }

    public class ClientStatisticsDto
    {
        public int Consultations { get; set; }
        public int Exams { get; set; }
        public int Surgeries { get; set; }
        public int Procedures { get; set; }
        public int CanceledOrMissedAppointments { get; set; }
    }
}

