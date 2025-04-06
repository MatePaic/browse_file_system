using Core.Entities;
using Infrastructure.Configurations;
using Microsoft.EntityFrameworkCore;
using System.Xml;

namespace Infrastructure.Data
{
    public class StoreContext(DbContextOptions options) : DbContext(options)
    {
        public DbSet<Folder> Folders { get; set; }
        public DbSet<FileItem> Files { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(FoldersConfiguration).Assembly);

            modelBuilder.Entity<FileItem>().HasKey(f => f.Id); // This is crucial
            modelBuilder.Entity<FileItem>()
                .HasOne(f => f.Folder)
                .WithMany(f => f.Files)
                .HasForeignKey(f => f.FolderId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
