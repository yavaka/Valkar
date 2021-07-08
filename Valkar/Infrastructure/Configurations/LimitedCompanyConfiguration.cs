namespace Infrastructure.Configurations
{
    using Infrastructure.Models;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using static Infrastructure.Common.ModelConstants;

    internal class LimitedCompanyConfiguration : IEntityTypeConfiguration<LimitedCompany>
    {
        public void Configure(EntityTypeBuilder<LimitedCompany> builder)
        {
            // Id
            builder.HasKey(i => i.Id);

            // Company name
            builder.Property(cn => cn.CompanyName)
                .HasMaxLength(MAX_COMPANY_NAME_LENGTH)
                .IsRequired();

            // Company registration number
            builder.Property(rn => rn.CompanyRegistrationNumber)
                .HasMaxLength(FIXED_COMPANY_REGISTRATION_NUMBER)
                .IsRequired();
        }
    }
}
