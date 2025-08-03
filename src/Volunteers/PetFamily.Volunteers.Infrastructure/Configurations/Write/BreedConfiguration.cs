using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetFamily.Infrastructure;
using PetFamily.SharedKernel;
using PetFamily.Volunteers.Domain.SpeciesManagement.Entities;
using PetFamily.Volunteers.Domain.SpeciesManagement.IDs;

namespace PetFamily.Volunteers.Infrastructure.Configurations.Write;

internal class BreedConfiguration : IEntityTypeConfiguration<Breed>
{
	public void Configure(EntityTypeBuilder<Breed> builder)
	{
		builder.ToTable("breeds");

		builder.HasKey(p => p.Id);

		builder.Property(p => p.Id)
			.HasConversion(
				id => id.Value,
				value => BreedId.Create(value))
			.IsRequired()
			.HasColumnName("id");

		builder.Property(p => p.Name)
			.IsRequired()
			.HasMaxLength(Constants.MAX_LOW_TEXT_LENGHT);
	}
}
