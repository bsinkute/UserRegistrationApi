using Microsoft.EntityFrameworkCore;
using UserRegistrationApi.Domain.Models;
using UserRegistrationApi.Infrastructure.Database.Configuration;

namespace UserRegistrationApi.Infrastructure.Database
{
    public class UserRegistrationDbContext : DbContext
    {
        public UserRegistrationDbContext(DbContextOptions<UserRegistrationDbContext> options) : base(options) { }
        public DbSet<User> Users { get; set; }
        public DbSet<PersonalInformation> PersonalInformation { get; set; }
        public DbSet<Address> Address { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new UserConfiguration());
            modelBuilder.ApplyConfiguration(new PersonalInformationConfiguration());
            modelBuilder.ApplyConfiguration(new AddressConfiguration());
        }
    }
}
