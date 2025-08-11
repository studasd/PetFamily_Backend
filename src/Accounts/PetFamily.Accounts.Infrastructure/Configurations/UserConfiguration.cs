using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetFamily.Accounts.Domain;
using PetFamily.SharedKernel.ValueObjects;
using System.Text.Json;

namespace PetFamily.Accounts.Infrastructure.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
	public void Configure(EntityTypeBuilder<User> builder)
	{
		builder
			.ToTable("users");

		builder.OwnsMany(p => p.SocialNetworks,
			pb =>
			{
				pb.ToJson("social_networks");

				pb.Property(s => s.Name)
				.IsRequired();

				pb.Property(s => s.Link)
				.IsRequired();
			});
	}
}
