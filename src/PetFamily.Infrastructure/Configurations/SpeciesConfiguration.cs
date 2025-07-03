using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetFamily.Domain.PetEntities;
using PetFamily.Infrastructure.Shared;

namespace PetFamily.Infrastructure.Configurations;

internal class SpeciesConfiguration : IEntityTypeConfiguration<Species>
{
	public void Configure(EntityTypeBuilder<Species> builder)
	{
		builder.ToTable("species");

		builder.HasKey(p => p.Id);

		builder.Property(p => p.Name)
			.IsRequired()
			.HasMaxLength(Constants.MAX_LOW_TEXT_LENGHT);

		builder.HasMany(p => p.Breeds)
			.WithOne()
			.HasForeignKey("breed_id")
			.IsRequired()
			.OnDelete(DeleteBehavior.NoAction);
	}
}