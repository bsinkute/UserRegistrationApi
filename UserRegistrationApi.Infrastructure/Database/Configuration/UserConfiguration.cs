using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UserRegistrationApi.Domain.Models;

namespace UserRegistrationApi.Infrastructure.Database.Configuration
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder) 
        {
            builder.ToTable("Users");
            // PK
            builder.HasKey(u => u.UserId);

            builder.Property(u => u.UserId)
                .IsRequired()
                .ValueGeneratedNever();
            builder.Property(u => u.Username)
                .IsRequired()
                .HasMaxLength(50);
            builder.Property(u => u.Password)
                .IsRequired()
                .HasMaxLength(100);
            builder.Property(u => u.Salt)
                .IsRequired()
                .HasMaxLength(100);
            builder.Property(u => u.Role)
                .IsRequired()
                .HasMaxLength(50);

            builder.HasOne(u => u.PersonalInformation)
                .WithOne(u => u.User)
                .HasForeignKey<PersonalInformation>(x => x.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
