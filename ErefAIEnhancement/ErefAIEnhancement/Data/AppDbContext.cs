using Microsoft.EntityFrameworkCore;
using ErefAIEnhancement.Models;

namespace ErefAIEnhancement.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<Professor> Professors { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .Property(u => u.Id)
                .HasDefaultValueSql("gen_random_uuid()");

            modelBuilder.Entity<Role>()
                .Property(r => r.Id)
                .HasDefaultValueSql("gen_random_uuid()");

            modelBuilder.Entity<Student>()
                .Property(s => s.Id)
                .HasDefaultValueSql("gen_random_uuid()");

            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Student>()
                .HasOne(s => s.User)
                .WithOne(u => u.Student)
                .HasForeignKey<Student>(s => s.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Student>()
                .HasIndex(s => s.UserId)
                .IsUnique();

            modelBuilder.Entity<Student>()
                .Property(s => s.Department)
                .HasConversion<string>();

            modelBuilder.Entity<Student>()
                .Property(s => s.YearOfStudy)
                .HasConversion<string>();

            modelBuilder.Entity<Professor>()
                .HasOne(p => p.User)
                .WithOne(u => u.Professor)
                .HasForeignKey<Professor>(p => p.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}