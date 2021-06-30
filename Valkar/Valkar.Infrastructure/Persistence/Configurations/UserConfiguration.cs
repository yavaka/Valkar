namespace Valkar.Infrastructure.Persistence.Configurations
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using Valkar.Domain.Models;

    using static Domain.Models.ModelConstants.User;

    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            // First name
            builder
                .Property(fn => fn.FirstName)
                .HasMaxLength(MAX_NAME_LENGTH);

            // Last name
            builder
                .Property(ln => ln.LastName)
                .HasMaxLength(MAX_NAME_LENGTH);

        }
    }
}
