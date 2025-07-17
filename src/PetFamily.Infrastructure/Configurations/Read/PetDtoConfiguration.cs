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
