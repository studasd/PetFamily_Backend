using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetFamily.Accounts.Domain;

namespace PetFamily.Accounts.Infrastructure.Configurations;

public class AdminAccountConfiguration : IEntityTypeConfiguration<AdminAccount>
{
	public void Configure(EntityTypeBuilder<AdminAccount> builder)
	{
		builder.ToTable("admin_accounts");

		builder.HasKey(x => x.Id);

		builder.Property(x => x.Id)
			.IsRequired();

		builder.Property(x => x.UserId)
			.IsRequired()
			.HasColumnName("user_id");

		builder
			.HasOne(a => a.User)
			.WithOne(u => u.AdminAccount)
			.HasForeignKey<AdminAccount>(a => a.UserId)
			.OnDelete(DeleteBehavior.Cascade);

		builder
			.ComplexProperty(a => a.FullName, fb =>
			{
				fb.Property(f => f.FirstName).HasColumnName("first_name");
				fb.Property(f => f.LastName).HasColumnName("last_name");
			});
	}
}
