using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetFamily.Domain.Shared;
using PetFamily.Domain.VolunteerManagement.Entities;
using PetFamily.Domain.VolunteerManagement.IDs;

namespace PetFamily.Infrastructure.Configurations.Write;
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

		builder.Property(p => p.Description)
			.IsRequired()
			.HasMaxLength(Constants.MAX_HIGHT_TEXT_LENGHT);

		builder.Property(p => p.Color)
			.IsRequired()
			.HasMaxLength(Constants.MAX_LOW_TEXT_LENGHT);

		builder.Property(p => p.HealthInfo)
			.IsRequired(false)
			.HasMaxLength(Constants.MAX_LOW_TEXT_LENGHT);

		builder.ComplexProperty(p => p.Position,
			x =>
			{
				x.Property(f => f.Value)
					.IsRequired()
					.HasColumnName("position");
			});

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
				pb.ToJson("phones");

				pb.Property(s => s.PhoneNumber)
					.IsRequired()
					.HasMaxLength(Constants.MAX_LOW_TEXT_LENGHT)
					.HasColumnName("phones");
			});

		builder.OwnsMany(p => p.FileStorages,
			pb =>
			{
				pb.ToJson("file_storages");

				pb.Property(s => s.PathToStorage)
					.IsRequired()
					.HasMaxLength(Constants.MAX_HIGHT_TEXT_LENGHT)
					.HasColumnName("files");
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

		builder.ComplexProperty(p => p.BankingDetails,
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

		builder.ComplexProperty(p => p.PetType, 
			x =>
			{
				x.Property(p => p.BreedId)
				.IsRequired()
				.HasColumnName("breed_id");

				x.Property(p => p.SpeciesId)
				.IsRequired()
				.HasColumnName("species_id");
			});

		builder.Property(v => v.IsSoftDeleted)
			.IsRequired()
			.HasColumnName("is_soft_deleted");
	}
}