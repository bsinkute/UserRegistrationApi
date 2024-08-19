using Microsoft.EntityFrameworkCore;
using UserRegistrationApi.Domain.Models;
using UserRegistrationApi.Infrastructure.Database.Configuration;

namespace UserRegistrationApi.Infrastructure.Database
{
    public class UserRegistrationDbContext : DbContext
    {
        public UserRegistrationDbContext(DbContextOptions<UserRegistrationDbContext> options) : base(options) { }
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new UserConfiguration());
        }
    }
}
