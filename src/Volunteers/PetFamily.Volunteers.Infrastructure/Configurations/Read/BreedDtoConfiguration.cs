using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetFamily.Core.DTOs;

namespace PetFamily.Volunteers.Infrastructure.Configurations.Read;

public class BreedDtoConfiguration : IEntityTypeConfiguration<BreedDto>
{
	public void Configure(EntityTypeBuilder<BreedDto> builder)
	{
		builder.ToTable("breeds");

		builder.HasKey(v => v.Id);
	}
}
