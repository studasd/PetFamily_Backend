using Microsoft.AspNetCore.Identity;

namespace PetFamily.Accounts.Domain;

public class User : IdentityUser<Guid>
{
	private User()
	{
		
	}

	public List<SocialNetwork> SocialNetworks { get; set; } = [];

	public IReadOnlyList<Role> Roles => roles;
	List<Role> roles = [];

	public static User CreateAdmin(string userName, string email, Role role)
	{
		var user = new User
		{
			UserName = userName,
			Email = email,
			EmailConfirmed = true,
			roles = [role]
		};
		
		return user;
	}

	public static User CreateUser(string userName, string email)
	{
		var user = new User
		{
			UserName = userName,
			Email = email,
		};

		return user;
	}
}
