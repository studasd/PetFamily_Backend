using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetFamily.Accounts.Domain;

namespace PetFamily.Accounts.Infrastructure.Configurations;

public class VolunteerAccountConfiguration : IEntityTypeConfiguration<VolunteerAccount>
{
	public void Configure(EntityTypeBuilder<VolunteerAccount> builder)
	{
		builder.ToTable("volunteer_accounts");

		builder.HasKey(x => x.Id);

		builder.Property(x => x.Id)
			.IsRequired();


		builder.Property(x => x.UserId)
			.IsRequired()
			.HasColumnName("user_id");

		builder.Property(x => x.Certificates)
			.IsRequired(false)
			.HasColumnName("certificates");

		builder.Property(x => x.Experience)
			.IsRequired()
			.HasColumnName("experience");

		builder.Property(x => x.Requisite)
			.IsRequired(false)
			.HasColumnName("requisites");
	}
}
