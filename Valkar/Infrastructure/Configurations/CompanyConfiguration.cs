namespace Infrastructure.Configurations
{
    using Infrastructure.Models;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using static Infrastructure.Common.ModelConstants;

    internal class CompanyConfiguration : IEntityTypeConfiguration<Company>
    {
        public void Configure(EntityTypeBuilder<Company> builder)
        {
            // Id
            builder.HasKey(i => i.Id);

            // Company name
            builder.Property(i => i.Name).HasMaxLength(MAX_COMPANY_NAME_LENGTH);

            // Address
            builder.Property(o => o.OfficeAddress).HasMaxLength(MAX_ADDRESS_LENGTH);

            // Postcode
            builder.Property(p => p.OfficePostCode).IsRequired();

            // Phone number
            builder.Property(p => p.PhoneNumber).IsRequired();
        }
    }
}
