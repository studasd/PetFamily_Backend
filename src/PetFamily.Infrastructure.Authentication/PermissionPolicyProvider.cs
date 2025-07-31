using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;

namespace PetFamily.Infrastructure.Authentication;

public class PermissionPolicyProvider : IAuthorizationPolicyProvider
{
	public Task<AuthorizationPolicy?> GetPolicyAsync(string policyName)
	{
		if(String.IsNullOrEmpty(policyName))
			return Task.FromResult<AuthorizationPolicy?>(null);

		var policy = new AuthorizationPolicyBuilder()
			.AddRequirements(new PermissionAttribute(policyName))
			.Build();

		return Task.FromResult<AuthorizationPolicy?>(policy);
	}

	public Task<AuthorizationPolicy> GetDefaultPolicyAsync() =>
		Task.FromResult<AuthorizationPolicy>(new AuthorizationPolicyBuilder()
			.RequireAuthenticatedUser()
			.Build());

	public Task<AuthorizationPolicy?> GetFallbackPolicyAsync() =>
		Task.FromResult<AuthorizationPolicy?>(null);

}
