namespace Infrastructure.Configurations
{
    using Infrastructure.Models;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    internal class WorkingDayConfiguration : IEntityTypeConfiguration<WorkingDay>
    {
        public void Configure(EntityTypeBuilder<WorkingDay> builder)
        {
            // Id 
            builder.HasKey(i => i.Id);

            // Date
            builder.Property(d => d.Date).IsRequired();

            // Time in
            builder.Property(ti => ti.TimeIn).IsRequired();

            // Break
            builder.Property(b => b.Break).IsRequired();

            // Time out
            builder.Property(to => to.TimeOut).IsRequired();

            // Total hours
            builder.Property(th => th.TotalHours);

            // Driver one to many
            builder.HasOne(d => d.Driver)
                .WithMany(wd => wd.WorkedDays)
                .HasForeignKey(d => d.DriverId);
        }
    }
}
