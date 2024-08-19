using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using UserRegistrationApi.Domain.Models;

namespace UserRegistrationApi.Infrastructure.Database.Configuration
{
    public class AddressConfiguration : IEntityTypeConfiguration<Address>
    {
        public void Configure(EntityTypeBuilder<Address> builder)
        {
            builder.ToTable("Address");
            // PK
            builder.HasKey(x => x.AddressId);

            builder.Property(x => x.AddressId)
                .IsRequired()
                .ValueGeneratedNever();
            builder.Property(x => x.City)
                .IsRequired()
                .HasMaxLength(100);
            builder.Property(x => x.Street)
                .IsRequired()
                .HasMaxLength(100);
            builder.Property(x => x.HouseNumber)
                .IsRequired()
                .HasMaxLength(100);
            builder.Property(x => x.ApartmentNumber)
                .IsRequired()
                .HasMaxLength(100);

            builder.HasOne(x => x.PersonalInformation)
                .WithOne(x => x.Address)
                .HasForeignKey<Address>(x => x.AddressId);
        }
    }
}
