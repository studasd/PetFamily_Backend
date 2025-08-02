using Microsoft.AspNetCore.Identity;

namespace PetFamily.Accounts.Domain;

public class User : IdentityUser<Guid>
{
	public List<SocialNetwork> SocialNetworks { get; set; } = [];
}
