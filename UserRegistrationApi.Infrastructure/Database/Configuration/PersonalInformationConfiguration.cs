using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using UserRegistrationApi.Domain.Models;

namespace UserRegistrationApi.Infrastructure.Database.Configuration
{
    public class PersonalInformationConfiguration : IEntityTypeConfiguration<PersonalInformation>
    {
        public void Configure(EntityTypeBuilder<PersonalInformation> builder)
        {
            builder.ToTable("PersonalInformation");
            // PK
            builder.HasKey(x => x.PersonalInformationId);

            builder.Property(x => x.PersonalInformationId)
                .IsRequired()
                .ValueGeneratedNever();
            builder.Property(x => x.FirstName)
                .IsRequired()
                .HasMaxLength(150);
            builder.Property(x => x.Surname)
                .IsRequired()
                .HasMaxLength(150);
            builder.Property(x => x.PersonalIdentificationNumber)
                .IsRequired()
                .HasMaxLength(50);
            builder.Property(x => x.PhoneNumber)
                .IsRequired()
                .HasMaxLength(20);
            builder.Property(x => x.Email)
                .IsRequired()
                .HasMaxLength(100);
            builder.Property(x => x.ProfilePicture)
                .IsRequired();

            builder.HasOne(x => x.User)
                .WithOne(x => x.PersonalInformation)
                .HasForeignKey<PersonalInformation>(x => x.PersonalInformationId);

            builder.HasOne(x => x.Address)
                .WithOne(x => x.PersonalInformation)
                .HasForeignKey<PersonalInformation>(x => x.PersonalInformationId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
