using Microsoft.AspNetCore.Identity;

namespace PetFamily.Application.Authorization.DataModels;

public class User : IdentityUser<Guid>
{
	public List<SocialNetwork> SocialNetworks { get; set; } = [];
}
