using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetFamily.Core.DTOs;

namespace PetFamily.Volunteers.Infrastructure.Configurations.Read;

public class SpeciesDtoConfiguration : IEntityTypeConfiguration<SpeciesDto>
{
	public void Configure(EntityTypeBuilder<SpeciesDto> builder)
	{
		builder.ToTable("species");

		builder.HasKey(v => v.Id);

		builder.HasMany(x => x.Breeds)
			.WithOne()
			.HasForeignKey(x => x.SpeciesId);
	}
}
