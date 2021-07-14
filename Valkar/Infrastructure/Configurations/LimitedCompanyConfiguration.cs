namespace Infrastructure.Configurations
{
    using Infrastructure.Models;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    internal class LimitedCompanyConfiguration : IEntityTypeConfiguration<LimitedCompany>
    {
        public void Configure(EntityTypeBuilder<LimitedCompany> builder)
        {
            // Id
            builder.HasKey(i => i.Id);
        }
    }
}
