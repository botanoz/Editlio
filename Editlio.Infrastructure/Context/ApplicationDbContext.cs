using Editlio.Shared.Entities;
using Microsoft.EntityFrameworkCore;

namespace Editlio.Infrastructure.Context
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Page> Pages { get; set; }
        public DbSet<Shared.Entities.File> Files { get; set; }
        public DbSet<RealTimeUpdate> RealTimeUpdates { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; } // Opsiyonel

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Benzersiz Slug için
            modelBuilder.Entity<Page>()
                .HasIndex(p => p.Slug)
                .IsUnique();

            // Şifre korunması
            modelBuilder.Entity<Page>()
                .Property(p => p.PasswordHash)
                .IsRequired(false);

            // File ilişkilendirme
            modelBuilder.Entity<Shared.Entities.File>()
                .HasOne(f => f.Page)
                .WithMany(p => p.Files)
                .HasForeignKey(f => f.PageId);
        }
    }
}
