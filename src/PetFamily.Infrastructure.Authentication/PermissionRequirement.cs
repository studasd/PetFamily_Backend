using Microsoft.AspNetCore.Authorization;

namespace PetFamily.Infrastructure.Authentication;

public class PermissionRequirement : IAuthorizationRequirement
{
	public PermissionRequirement(string code)
	{
		Code = code;
	}

	public string Code { get; }
}
