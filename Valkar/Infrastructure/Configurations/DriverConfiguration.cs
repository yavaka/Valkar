namespace Infrastructure.Configurations
{
    using Infrastructure.Models;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using static Infrastructure.Common.ModelConstants;

    internal class DriverConfiguration : IEntityTypeConfiguration<Driver>
    {
        public void Configure(EntityTypeBuilder<Driver> builder)
        {
            // Id
            builder.HasKey(i => i.Id);

            // Title
            builder.Property(t => t.Title).IsRequired();

            // First names
            builder.Property(fn => fn.FirstNames)
                .HasMaxLength(MAX_NAME_LENGTH)
                .IsRequired();

            // Surname
            builder.Property(s => s.Surname)
                .HasMaxLength(MAX_NAME_LENGTH)
                .IsRequired();

            // Address
            builder.Property(a => a.Address)
                .HasMaxLength(MAX_ADDRESS_LENGTH)
                .IsRequired();

            // Postcode
            builder.Property(p => p.Postcode).IsRequired();

            // Phone number
            builder.Property(p => p.PhoneNumber).IsRequired();

            // Emergency contact one to one
            builder.HasOne(ec => ec.EmergencyContact)
                .WithOne(d => d.Driver)
                .HasForeignKey<EmergencyContact>(d => d.DriverId)
                .IsRequired();

            // Limited company one to one
            builder.HasOne(lc => lc.LimitedCompany)
                .WithOne(o => o.Owner)
                .HasForeignKey<LimitedCompany>(o => o.OwnerId);

            // User one to one
            builder.HasOne(u => u.User)
                .WithOne(d => d.Driver)
                .HasForeignKey<Driver>(u => u.UserId);
        }
    }
}
