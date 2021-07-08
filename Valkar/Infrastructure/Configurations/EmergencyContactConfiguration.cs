namespace Infrastructure.Configurations
{
    using Infrastructure.Models;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using static Infrastructure.Common.ModelConstants;

    internal class EmergencyContactConfiguration : IEntityTypeConfiguration<EmergencyContact>
    {
        public void Configure(EntityTypeBuilder<EmergencyContact> builder)
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

            // Relationship
            builder.Property(r => r.Relationship)
                .HasMaxLength(MAX_RELATIONSHIP_LENGTH)
                .IsRequired();
        }
    }
}
