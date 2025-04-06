using Core.Entities;
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
            modelBuilder.Entity<Folder>().HasKey(f => f.Id); // This is crucial
            modelBuilder.Entity<Folder>()
                .HasOne(f => f.ParentFolder)
                .WithMany(f => f.Subfolders)
                .HasForeignKey(f => f.ParentFolderId)
                .OnDelete(DeleteBehavior.Restrict);
            //modelBuilder.Entity<Folder>()
            //    .HasIndex(f => f.Path);
            //modelBuilder.Entity<Folder>()
            //    .HasIndex(e => e.Name)
            //    .IsUnique();


            modelBuilder.Entity<FileItem>().HasKey(f => f.Id); // This is crucial
            modelBuilder.Entity<FileItem>()
                .HasOne(f => f.Folder)
                .WithMany(f => f.Files)
                .HasForeignKey(f => f.FolderId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
