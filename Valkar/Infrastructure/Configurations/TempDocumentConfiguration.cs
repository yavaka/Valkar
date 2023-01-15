namespace Infrastructure.Configurations
{
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using Microsoft.EntityFrameworkCore;
    using Infrastructure.Models;

    internal class TempDocumentConfiguration : IEntityTypeConfiguration<TempDocument>
    {
        public void Configure(EntityTypeBuilder<TempDocument> builder)
        {
            // Id
            builder.HasKey(i => i.Id);

            // SentTo one to many
            builder.HasOne(u => u.SentTo)
                .WithMany(d => d.ReceivedDocuments)
                .HasForeignKey(u => u.SentToId);
        }
    }
}
