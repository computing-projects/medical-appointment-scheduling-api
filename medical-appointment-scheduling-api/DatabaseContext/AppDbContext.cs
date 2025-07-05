using medical_appointment_scheduling_api.Models;
using Microsoft.EntityFrameworkCore;

namespace medical_appointment_scheduling_api.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DoctorHealthPlans>()
                .HasKey(dhp => new { dhp.DoctorId, dhp.HealthPlan});
        }
        public DbSet<Appointments> Appointments { get; set; }
        public DbSet<Clients> Clients { get; set; }
        public DbSet<Clinics> Clinics { get; set; }
        public DbSet<ClinicUsers> ClinicUsers { get; set; }
        public DbSet<DoctorHealthPlans> DoctorHealthPlans { get; set; }
        public DbSet<Doctors> Doctors { get; set; }
        public DbSet<Notifications> Notifications { get; set; }
        public DbSet<Reviews> Reviews { get; set; }
        public DbSet<Schedules> Schedules { get; set; }
        public DbSet<Users> Users{ get; set; }
        public DbSet<Waitlist> Waitlist { get; set; }
    }
}
