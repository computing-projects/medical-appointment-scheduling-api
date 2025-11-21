using System;
using medical_appointment_scheduling_api.Models;
using Microsoft.EntityFrameworkCore;

namespace medical_appointment_scheduling_api.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configure enum conversions
            modelBuilder.Entity<Users>()
                .Property(u => u.Role)
                .HasConversion<string>();

            modelBuilder.Entity<Users>()
                .Property(u => u.CreatedAt)
                .ValueGeneratedOnAdd();

            modelBuilder.Entity<Users>()
                .Property(u => u.UpdatedAt)
                .ValueGeneratedOnAddOrUpdate();

            modelBuilder.Entity<Users>()
                .Property(u => u.DeletedAt)
                .IsRequired(false);

            // Explicitly configure Clinics.DeletedAt as nullable
            modelBuilder.Entity<Clinics>()
                .Property(c => c.DeletedAt)
                .IsRequired(false);

            modelBuilder.Entity<Doctors>()
                .Property(d => d.Specialty)
                .HasConversion<string>();

            modelBuilder.Entity<Appointments>()
                .Property(a => a.AppointmentType)
                .HasConversion<string>();

            modelBuilder.Entity<Appointments>()
                .Property(a => a.Status)
                .HasConversion<string>();

            // Explicitly configure Appointments.CanceledAt as nullable
            modelBuilder.Entity<Appointments>()
                .Property(a => a.CanceledAt)
                .IsRequired(false);

            modelBuilder.Entity<Schedules>()
                .Property(s => s.Weekday)
                .HasConversion(
                    v => v.ToString().ToLowerInvariant(),
                    v => (SystemEnums.Weekday)Enum.Parse(typeof(SystemEnums.Weekday), v, true)
                );

            modelBuilder.Entity<Notifications>()
                .Property(n => n.Channel)
                .HasConversion<string>();

            modelBuilder.Entity<Notifications>()
                .Property(n => n.Category)
                .HasConversion<string>();

            modelBuilder.Entity<ClinicUsers>()
                .Property(cu => cu.Role)
                .HasConversion<string>();

            modelBuilder.Entity<Waitlist>()
                .Property(w => w.Status)
                .HasConversion(
                    v => v.ToString().ToLowerInvariant(),
                    v => (SystemEnums.WaitlistStatus)Enum.Parse(typeof(SystemEnums.WaitlistStatus), v, true)
                );

            // Convert AppointmentType enum: InPerson -> "in_person", Online -> "online"
            // This matches the database CHECK constraint and DefaultValue attributes
            modelBuilder.Entity<Waitlist>()
                .Property(w => w.AppointmentType)
                .HasConversion(
                    v => v == SystemEnums.AppointmentType.InPerson ? "in_person" : "online",
                    v => v == "in_person" ? SystemEnums.AppointmentType.InPerson : SystemEnums.AppointmentType.Online
                );
        }

        public DbSet<Anamnese> Anamnese { get; set; }
        public DbSet<Appointments> Appointments { get; set; }
        public DbSet<ClientHealthPlans> ClientHealthPlans { get; set; }
        public DbSet<Clients> Clients { get; set; }
        public DbSet<Clinics> Clinics { get; set; }
        public DbSet<ClinicUsers> ClinicUsers { get; set; }
        public DbSet<DoctorHealthPlans> DoctorHealthPlans { get; set; }
        public DbSet<Doctors> Doctors { get; set; }
        public DbSet<HealthPlans> HealthPlans { get; set; }
        public DbSet<Notifications> Notifications { get; set; }
        public DbSet<Reviews> Reviews { get; set; }
        public DbSet<Schedules> Schedules { get; set; }
        public DbSet<Users> Users { get; set; }
        public DbSet<Waitlist> Waitlist { get; set; }
    }
}
