using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.Identity;
using PetFamily.SharedKernel;
using PetFamily.SharedKernel.ValueObjects;

namespace PetFamily.Accounts.Domain;

public class User : IdentityUser<Guid>
{
	private User()
	{
		
	}

	public IReadOnlyList<SocialNetwork> SocialNetworks => socialNetworks;
	private readonly List<SocialNetwork> socialNetworks = [];

	public IReadOnlyList<Role> Roles => roles;
	List<Role> roles = [];


	public AdminAccount? AdminAccount { get; private set; }

	public VolunteerAccount? VolunteerAccount { get; private set; }

	public ParticipantAccount? ParticipantAccount { get; private set; }


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


	public UnitResult<Error> AddSocialNetworks(IEnumerable<SocialNetwork> socNetworks)
	{
		socialNetworks.AddRange(socNetworks);

		return Result.Success<Error>();
	}

	public void UpdateSocialNetworks(IEnumerable<SocialNetwork> socialNetworks)
	{
		this.socialNetworks.Clear();
		this.socialNetworks.AddRange(socialNetworks);
	}
}
