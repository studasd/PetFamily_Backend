using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetFamily.Accounts.Domain;

namespace PetFamily.Accounts.Infrastructure.Configurations;

public class RolePermissionConfiguration : IEntityTypeConfiguration<RolePermission>
{
	public void Configure(EntityTypeBuilder<RolePermission> builder)
	{
		builder
			.ToTable("role_permissions");

		builder
			.HasOne(x => x.Role)
			.WithMany(r => r.RolePermissions)
			.HasForeignKey(r => r.RoleId);

		builder
			.HasOne(x => x.Permission)
			.WithMany()
			.HasForeignKey(r => r.PermissionId);

		builder
			.HasKey(x => new { x.RoleId, x.PermissionId });
	}
}