namespace Infrastructure.Configurations
{
    using Infrastructure.Models;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    internal class FileConfiguration : IEntityTypeConfiguration<File>
    {
        public void Configure(EntityTypeBuilder<File> builder)
        {
            // Id
            builder.HasKey(i => i.Id);

            // UploadedBy one to many
            builder.HasOne(u => u.UploadedBy)
                .WithMany(d => d.Documents)
                .HasForeignKey(u => u.UploadedById);
        }
    }
}
