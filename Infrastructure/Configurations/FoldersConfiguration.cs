using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations
{
    public class FoldersConfiguration : IEntityTypeConfiguration<Folder>
    {
        public void Configure(EntityTypeBuilder<Folder> builder)
        {
            // Primary Key
            builder.HasKey(f => f.Id);

            // Properties
            builder.Property(f => f.Name)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(f => f.Path)
                .IsRequired()
                .HasMaxLength(500);

            // Relationships
            builder.HasOne(f => f.ParentFolder)
                .WithMany(f => f.Subfolders)
                .HasForeignKey(f => f.ParentFolderId)
                .OnDelete(DeleteBehavior.Restrict); // Prevent cascade delete

            // Indexes
            builder.HasIndex(f => f.Path)
                .IsUnique();

            builder.HasIndex(f => f.ParentFolderId);
        }
    }
}