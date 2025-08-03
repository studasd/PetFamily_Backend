using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;

namespace PetFamily.Web.Authorization;

public class PermissionPolicyProvider : IAuthorizationPolicyProvider
{
	public Task<AuthorizationPolicy?> GetPolicyAsync(string policyName)
	{
		if(string.IsNullOrEmpty(policyName))
			return Task.FromResult<AuthorizationPolicy?>(null);

		var policy = new AuthorizationPolicyBuilder()
			.RequireAuthenticatedUser()
			.AddRequirements(new PermissionAttribute(policyName))
			.Build();

		return Task.FromResult<AuthorizationPolicy?>(policy);
	}

	public Task<AuthorizationPolicy> GetDefaultPolicyAsync() =>
		Task.FromResult(new AuthorizationPolicyBuilder()
			.RequireAuthenticatedUser()
			.Build());

	public Task<AuthorizationPolicy?> GetFallbackPolicyAsync() =>
		Task.FromResult<AuthorizationPolicy?>(null);

}
