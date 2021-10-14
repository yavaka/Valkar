namespace Infrastructure
{
    using System.Reflection;
    using Infrastructure.Models;
    using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore;

    public class ValkarDbContext : IdentityDbContext<User>
    {
        public ValkarDbContext(DbContextOptions<ValkarDbContext> options)
            : base(options)
        {
        }

        public DbSet<Driver> Drivers{ get; set; }
        public DbSet<EmergencyContact> EmergencyContacts { get; set; }
        public DbSet<LimitedCompany> LimitedCompanies { get; set; }
        public DbSet<LicenceCategory> DriversLicenceCategories { get; set; }
        public DbSet<File> Files { get; set; }
        public DbSet<WorkingDay> WorkedDays { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            base.OnModelCreating(builder);
        }
    }
}
