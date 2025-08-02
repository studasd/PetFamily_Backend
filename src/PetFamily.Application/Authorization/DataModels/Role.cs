using Microsoft.AspNetCore.Identity;

namespace PetFamily.Application.Authorization.DataModels;

public class Role : IdentityRole<Guid>
{
	public List<RolePermission> RolePermissions {  get; set; }
}
