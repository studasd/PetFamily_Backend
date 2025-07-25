﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetFamily.Domain.Shared;
using PetFamily.Domain.SpeciesManagement.Entities;
using PetFamily.Domain.SpeciesManagement.IDs;

namespace PetFamily.Infrastructure.Configurations.Write;

internal class SpeciesConfiguration : IEntityTypeConfiguration<Species>
{
	public void Configure(EntityTypeBuilder<Species> builder)
	{
		builder.ToTable("species");

		builder.HasKey(p => p.Id);

		builder.Property(p => p.Id)
			.HasConversion(
				id => id.Value,
				value => SpeciesId.Create(value))
			.IsRequired()
			.HasColumnName("id");

		builder.Property(p => p.Name)
			.IsRequired()
			.HasMaxLength(Constants.MAX_LOW_TEXT_LENGHT);


		builder.HasMany(p => p.Breeds)
			.WithOne()
			.HasForeignKey("species_id")
			.IsRequired()
			.OnDelete(DeleteBehavior.Cascade);
	}
}