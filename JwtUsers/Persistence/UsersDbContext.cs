using Members.Entities;
using Microsoft.EntityFrameworkCore;

namespace JwtUsers.Persistence
{
    public class UsersDbContext : DbContext
    {
        public UsersDbContext(DbContextOptions<UsersDbContext> options)
            : base(options) { }

        public DbSet<User> Users => Set<User>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasIndex(u => u.Email).IsUnique(); // Prevent duplicate emails
            });

            base.OnModelCreating(modelBuilder);
        }
    }
}
