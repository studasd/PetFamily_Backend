using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetFamily.Domain.PetEntities;
using PetFamily.Infrastructure.Shared;

namespace PetFamily.Infrastructure.Configurations;
internal class PetConfiguration : IEntityTypeConfiguration<Pet>
{
	public void Configure(EntityTypeBuilder<Pet> builder)
	{
		builder.ToTable("pets");

		builder.HasKey(p => p.Id);

		builder.Property(p => p.Id)
			.HasConversion(
				id => id.Value,
				value => PetId.Create(value))
			.IsRequired()
			.HasColumnName("id");

		builder.Property(p => p.Name)
			.IsRequired()
			.HasMaxLength(Constants.MAX_LOW_TEXT_LENGHT);

		builder.Property(p => p.Type)
			.IsRequired()
			.HasConversion<string>()
			.HasDefaultValue(default)
			.HasColumnName("pet_type");

		builder.Property(p => p.Description)
			.IsRequired()
			.HasMaxLength(Constants.MAX_HIGHT_TEXT_LENGHT);

		builder.Property(p => p.Color)
			.IsRequired()
			.HasMaxLength(Constants.MAX_LOW_TEXT_LENGHT);

		builder.Property(p => p.HealthInfo)
			.IsRequired(false)
			.HasMaxLength(Constants.MAX_LOW_TEXT_LENGHT);

		builder.ComplexProperty(p => p.Address,
			x =>
			{
				x.Property(f => f.Country)
					.IsRequired(false)
					.HasMaxLength(Constants.MAX_LOW_TEXT_LENGHT)
					.HasColumnName("addr_country");

				x.Property(f => f.City)
					.IsRequired(false)
					.HasMaxLength(Constants.MAX_LOW_TEXT_LENGHT)
					.HasColumnName("addr_city");

				x.Property(f => f.Street)
					.IsRequired(false)
					.HasMaxLength(Constants.MAX_LOW_TEXT_LENGHT)
					.HasColumnName("addr_street");

				x.Property(f => f.HouseNumber)
					.IsRequired()
					.HasColumnName("addr_house_number");

				x.Property(f => f.HouseLiter)
					.IsRequired(false)
					.HasMaxLength(Constants.MAX_LOW_TEXT_LENGHT)
					.HasColumnName("addr_house_liter");

				x.Property(f => f.Apartment)
					.IsRequired()
					.HasColumnName("addr_apartment");
			});

		builder.Property(p => p.Weight)
			.IsRequired();

		builder.Property(p => p.Height)
			.IsRequired();

		builder.OwnsMany(p => p.Phones,
			pb =>
			{
				pb.ToJson();

				pb.Property(s => s.phone)
					.IsRequired()
					.HasMaxLength(Constants.MAX_LOW_TEXT_LENGHT)
					.HasColumnName("phones");
			});

		builder.Property(p => p.IsNeutered)
			.IsRequired(false);

		builder.Property(p => p.IsVaccinated)
			.IsRequired(false);

		builder.Property(p => p.DateBirth)
			.IsRequired()
			.HasColumnName("date_birth");

		builder.Property(p => p.HelpStatus)
			.IsRequired()
			.HasConversion<string>()
			.HasDefaultValue(default)
			.HasColumnName("pet_status");

		builder.ComplexProperty(p => p.BankingВetails,
			x =>
			{
				x.Property(f => f.Name)
					.IsRequired(false)
					.HasMaxLength(Constants.MAX_LOW_TEXT_LENGHT)
					.HasColumnName("bank_name");

				x.Property(f => f.Description)
					.IsRequired(false)
					.HasMaxLength(Constants.MAX_LOW_TEXT_LENGHT)
					.HasColumnName("bank_description");
			});

		builder.Property(p => p.DateCreated)
			.IsRequired()
			.HasColumnName("date_created");

		builder.HasOne(p => p.Breed)
			.WithMany()
			.HasForeignKey("breed_id")
			.IsRequired()
			.OnDelete(DeleteBehavior.NoAction);

		builder.HasOne(p => p.Species)
			.WithMany()
			.HasForeignKey("specie_id")
			.IsRequired()
			.OnDelete(DeleteBehavior.NoAction);
	}
}