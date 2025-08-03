using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetFamily.Core.DTOs;

namespace PetFamily.Specieses.Infrastructure.Configurations.Read;

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
