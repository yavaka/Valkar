namespace Infrastructure.Configurations
{
    using Infrastructure.Models;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    internal class LicenceCategoryConfiguration : IEntityTypeConfiguration<LicenceCategory>
    {
        public void Configure(EntityTypeBuilder<LicenceCategory> builder)
        {
            // Id
            builder.HasKey(i => i.Id);
            
            // Category
            builder.Property(c => c.Category)
                .IsRequired();

            // Driver
            builder.HasOne(d => d.Driver)
                .WithMany(c => c.LicenceCategories)
                .HasForeignKey(d => d.DriverId);
        }
    }
}
