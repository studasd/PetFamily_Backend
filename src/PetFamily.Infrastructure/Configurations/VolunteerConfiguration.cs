using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetFamily.Domain.Shared;
using PetFamily.Domain.VolunteerManagement.IDs;
using PetFamily.Domain.VolunteerManagement.Entities;

namespace PetFamily.Infrastructure.Configurations;

public class VolunteerConfiguration : IEntityTypeConfiguration<Volunteer>
{
	public void Configure(EntityTypeBuilder<Volunteer> builder)
	{
		builder.ToTable("volunteers");

		builder.HasKey(v => v.Id);

		builder.Property(v => v.Id)
			.HasConversion(
				id => id.Value,
				value => VolunteerId.Create(value))
			.IsRequired(true)
			.HasColumnName("id");

		builder.ComplexProperty(p => p.Name,
			x =>
			{
				x.Property(f => f.Firstname)
					.IsRequired()
					.HasMaxLength(Constants.MAX_LOW_TEXT_LENGHT)
					.HasColumnName("first_name");

				x.Property(f => f.Lastname)
					.IsRequired()
					.HasMaxLength(Constants.MAX_LOW_TEXT_LENGHT)
					.HasColumnName("last_name");

				x.Property(f => f.Surname)
					.IsRequired()
					.HasMaxLength(Constants.MAX_LOW_TEXT_LENGHT)
					.HasColumnName("sur_name");
			});

		builder.Property(v => v.Email)
			.IsRequired()
			.HasMaxLength(Constants.MAX_LOW_TEXT_LENGHT);

		builder.Property(v => v.Description)
			.IsRequired()
			.HasMaxLength(Constants.MAX_HIGHT_TEXT_LENGHT);

		builder.Property(v => v.ExperienceYears)
			.IsRequired();

		builder.ComplexProperty(p => p.Phone,
			x =>
			{
				x.Property(f => f.phone)
					.IsRequired()
					.HasMaxLength(Constants.MAX_LOW_TEXT_LENGHT)
					.HasColumnName("phone");
			});

		builder.OwnsMany(p => p.BankingDetails,
			bb =>
			{
				bb.ToJson("banking_details");

				bb.Property(f => f.Name)
					.IsRequired(false)
					.HasMaxLength(Constants.MAX_LOW_TEXT_LENGHT)
					.HasColumnName("bank_name");

				bb.Property(f => f.Description)
					.IsRequired(false)
					.HasMaxLength(Constants.MAX_LOW_TEXT_LENGHT)
					.HasColumnName("bank_description");
			});


		builder.OwnsMany(p => p.SocialNetworks,
			pb =>
			{
				pb.ToJson("social_networks");

				pb.Property(s => s.Name)
				.IsRequired()
				.HasMaxLength(Constants.MAX_LOW_TEXT_LENGHT);

				pb.Property(s => s.Link)
				.IsRequired()
				.HasMaxLength(Constants.MAX_LOW_TEXT_LENGHT);

			});

		builder.HasMany(v => v.Pets)
			.WithOne()
			.HasForeignKey("volunteer_id")
			.OnDelete(DeleteBehavior.Cascade);

		builder.Property<bool>("isDeleted")
			.UsePropertyAccessMode(PropertyAccessMode.Field)
			.HasColumnName("id_deleted");
	}
}
