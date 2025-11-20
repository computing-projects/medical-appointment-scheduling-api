using System.ComponentModel;

namespace medical_appointment_scheduling_api.Models
{
    public class SystemEnums
    {
        public enum ETipoClinicUser : sbyte
        {
            [DefaultValue("doctor")] Doctor = 1,
            [DefaultValue("assistant")] Assistant = 2,
            [DefaultValue("admin")] Admin = 3
        }

        public enum Speciality : sbyte
        {
            [DefaultValue("Cardiology")] Cardiology = 1,
            [DefaultValue("Dermatology")] Dermatology = 2,
            [DefaultValue("Endocrinology")] Endocrinology = 3,
            [DefaultValue("Gastroenterology")] Gastroenterology = 4,
            [DefaultValue("Neurology")] Neurology = 5,
            [DefaultValue("Orthopedics")] Orthopedics = 6,
            [DefaultValue("Pediatrics")] Pediatrics = 7,
            [DefaultValue("Psychiatry")] Psychiatry = 8,
            [DefaultValue("General")] General = 9
        }

        public enum HealthPlans : sbyte
        {
            [DefaultValue("SUS")] SUS = 1,
            [DefaultValue("Unimed")] Unimed = 2,
            [DefaultValue("Bradesco")] Bradesco = 3,
            [DefaultValue("Amil")] Amil = 4,
            [DefaultValue("Other")] Other = 5
        }

        public enum AppointmentType : sbyte
        {
            [DefaultValue("in_person")] InPerson = 1,
            [DefaultValue("online")] Online = 2
        }

        public enum AppointmentStatus : sbyte
        {
            [DefaultValue("scheduled")] Scheduled = 1,
            [DefaultValue("completed")] Completed = 2,
            [DefaultValue("canceled")] Canceled = 3,
            [DefaultValue("no_show")] NoShow = 4
        }

        public enum AppointmentCategory : sbyte
        {
            [DefaultValue("consultation")] Consultation = 1,
            [DefaultValue("exam")] Exam = 2,
            [DefaultValue("surgery")] Surgery = 3,
            [DefaultValue("procedure")] Procedure = 4
        }

        public enum WaitlistStatus : sbyte
        {
            [DefaultValue("pending")] Pending = 1,
            [DefaultValue("confirmed")] Confirmed = 2,
            [DefaultValue("canceled")] Canceled = 3
        }

        public enum NotificationChannel : sbyte
        {
            [DefaultValue("email")] Email = 1,
            [DefaultValue("whatsapp")] Whatsapp = 2
        }

        public enum NotificationCategory : sbyte
        {
            [DefaultValue("reminder")] Reminder = 1,
            [DefaultValue("system")] System = 2,
            [DefaultValue("alert")] Alert = 3
        }

        public enum Weekday : sbyte
        {
            [DefaultValue("monday")] Monday = 1,
            [DefaultValue("tuesday")] Tuesday = 2,
            [DefaultValue("wednesday")] Wednesday = 3,
            [DefaultValue("thursday")] Thursday = 4,
            [DefaultValue("friday")] Friday = 5,
            [DefaultValue("saturday")] Saturday = 6,
            [DefaultValue("sunday")] Sunday = 7
        }

        public enum ChatStatus : sbyte
        {
            [DefaultValue("sent")] Sent = 1,
            [DefaultValue("delivered")] Delivered = 2,
            [DefaultValue("read")] Read = 3
        }
    }
}
