using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetFamily.Application.DTOs;

namespace PetFamily.Infrastructure.Configurations.Read;

public class PetDtoConfiguration : IEntityTypeConfiguration<PetDto>
{
	public void Configure(EntityTypeBuilder<PetDto> builder)
	{
		builder.ToTable("pets");

		builder.HasKey(p => p.Id);

		builder.Property(f => f.AddressCountry).HasColumnName("addr_country");

		builder.Property(f => f.AddressCity).HasColumnName("addr_city");

		builder.Property(f => f.AddressStreet).HasColumnName("addr_street");

		builder.Property(f => f.AddressHouseNumber).HasColumnName("addr_house_number");

		builder.Property(f => f.AddressHouseLiter).HasColumnName("addr_house_liter");

		builder.Property(f => f.AddressApartment).HasColumnName("addr_apartment");

		

		builder.Property(p => p.Type)
			.IsRequired()
			.HasConversion<string>()
			.HasDefaultValue(default)
			.HasColumnName("pet_type");

		builder.Property(p => p.HelpStatus)
			.IsRequired()
			.HasConversion<string>()
			.HasDefaultValue(default)
			.HasColumnName("pet_status");
	}
}
