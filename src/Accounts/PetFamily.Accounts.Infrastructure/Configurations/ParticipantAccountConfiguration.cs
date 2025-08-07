using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetFamily.Accounts.Domain;

namespace PetFamily.Accounts.Infrastructure.Configurations;

public class ParticipantAccountConfiguration : IEntityTypeConfiguration<ParticipantAccount>
{
	public void Configure(EntityTypeBuilder<ParticipantAccount> builder)
	{
		builder.ToTable("participant_accounts");

		builder.HasKey(x => x.Id);

		builder.Property(x => x.Id)
			.IsRequired();

		builder.Property(x => x.UserId)
			.IsRequired()
			.HasColumnName("user_id");

		builder.Property(p => p.FavoritePetId)
			.IsRequired(false)
			.HasColumnName("favorite_pet_id");
	}
}