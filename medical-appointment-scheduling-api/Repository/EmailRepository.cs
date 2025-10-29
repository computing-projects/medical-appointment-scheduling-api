using System.Net;
using System.Net.Mail;
using System.Linq;
using System.Threading.Tasks;
using medical_appointment_scheduling_api.Models.DTO;
using System.Security.Cryptography;
using Microsoft.OpenApi.Models;
using System.Text;
using medical_appointment_scheduling_api.Data;
using Microsoft.EntityFrameworkCore;
using medical_appointment_scheduling_api.Services;
using medical_appointment_scheduling_api.Repositories;
using Microsoft.Extensions.DependencyInjection;
using medical_appointment_scheduling_api.Models;

namespace medical_appointment_scheduling_api.Repositories
{
    public class EmailRepository
    {
        private readonly SmtpClient _smtpClient;
        private readonly AppDbContext _db;
        public EmailRepository(AppDbContext db)
        {
            _db = db;
            _smtpClient = new SmtpClient("smtp.your-email-provider.com")  // precisamos ter nosso email provider aqui
            {
                Port = 587, // Common SMTP port
                Credentials = new NetworkCredential("noreply@nosso.com", "Email"), // 
                EnableSsl = true // Enable SSL for secure connection
            };
        }

        public async Task SendAppointmentCreationEmailAsync(string to, string subject, string body, MailMessageInfo message)
        {
            var UserDoctor = await _db.Users.Where(W => W.Id == message.Doctor.UserId)
                                            .FirstOrDefaultAsync();
            var UserClient = await _db.Users.Where(W => W.Id == message.Client.UserId)
                                            .FirstOrDefaultAsync();

            if (UserDoctor == null)
                throw new Exception("Doctor not found");
            if (UserClient == null)
                throw new Exception("Client not found");

            var mailMessage = new MailMessage
            {
                From = new MailAddress("your-email@example.com"),
                Subject = $"Consulta Agendada Com Sucesso com Doutor {UserDoctor.Name}",
                Body = $"<p> Consulta de {message.Doctor.Specialty} </p>"
                     + $"<p> Dia: {message.AppointmentDate.ToString("dd/MM/yyyy")} </p>"
                     + $"<p> Hora: {message.AppointmentDate.ToString("HH:mm:ss")} </p>"
                     + $"<p> Cl�nica: {message.Clinic.Name} </p>"
                     + $"<p> Endere�o: {message.Clinic.Address} </p>"
                     + $"<p> Aguardamos ansiosamente seu comparecimento!! </p>",
                IsBodyHtml = true
            };

            mailMessage.To.Add(to);

            await SendEmailAsync(mailMessage);
        }

        public async Task<bool> SendEmailAsync(MailMessage mailMessage)
        {
            try
            {
                await _smtpClient.SendMailAsync(mailMessage);
                return true;
            }
            catch (SmtpException ex)
            {
                return false;
            }
        }

        public async Task SendNotificationAsync(Notifications notification)
        {
            /*
             Montar aqui a l�gica para enviar notifica��es, seja email ou mensagem SMS

             */

            switch (notification.Channel)
            {
                case SystemEnums.NotificationChannel.Email:
                    await SendEmailAsync(new MailMessage());
                    break;
                case SystemEnums.NotificationChannel.Whatsapp:
                    break;
                default:
                    throw new NotImplementedException("Notification channel not implemented");
            }
        }
    }
}
