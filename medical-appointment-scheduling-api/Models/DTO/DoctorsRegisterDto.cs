public class DoctorsRegisterDto
{
    public int Id { get; set; }
    public int UserId {get;set;}
    public string Photo { get; set; }
    public string Name { get; set; }
    public string Password { get; set; }
    public string Cpf { get; set; }
    public string Crm { get; set; }
    public string Cidade { get; set; }
    public string Estado { get; set; }
    public string Cep { get; set; }
    public string Role { get; set; }
    public List<string> Plans { get; set; }
    public List<string> Specialtys { get; set; }
    public string Email { get; set; }
    public string Phone { get; set; }
    public string Address { get; set; }
    public int AvgAppointmentTime { get; set; }

    // 'active' | 'inactive' -> Enum em C#
    public string Status { get; set; }

    public int PatientsCount { get; set; }
    public double Rating { get; set; }
}