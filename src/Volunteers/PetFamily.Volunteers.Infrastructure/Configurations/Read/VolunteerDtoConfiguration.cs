using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetFamily.Core.DTOs;
using System.Text.Json;

namespace PetFamily.Volunteers.Infrastructure.Configurations.Read;

public class VolunteerDtoConfiguration : IEntityTypeConfiguration<VolunteerDto>
{
	public void Configure(EntityTypeBuilder<VolunteerDto> builder)
	{
		builder.ToTable("volunteers");

		builder.HasKey(v => v.Id);

		builder.Property(f => f.Firstname).HasColumnName("first_name");
		builder.Property(f => f.Lastname).HasColumnName("last_name");
		builder.Property(f => f.Surname).HasColumnName("sur_name");
		builder.Property(f => f.ExperienceYears).HasColumnName("experience_years");
		builder.Property(f => f.IsSoftDeleted).HasColumnName("is_soft_deleted");
		builder.Property(f => f.DateDeletion).HasColumnName("date_deletion");

		builder.Property(p => p.BankingDetails)
			.HasConversion(
				files => JsonSerializer.Serialize(string.Empty, JsonSerializerOptions.Default),
				json => JsonSerializer.Deserialize<BankingDetailsDTO[]>(json, JsonSerializerOptions.Default)!
			)
			.HasColumnName("banking_details");

		builder.Property(p => p.SocialNetworks)
			.HasConversion(
				files => JsonSerializer.Serialize(string.Empty, JsonSerializerOptions.Default),
				json => JsonSerializer.Deserialize<SocialNetworkDTO[]>(json, JsonSerializerOptions.Default)!
			)
			.HasColumnName("social_networks");


		builder.HasMany(x => x.Pets)
			.WithOne()
			.HasForeignKey(x => x.VolunteerId);
	}
}